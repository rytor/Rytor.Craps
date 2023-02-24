using System.Net;
using Rytor.Libraries.Dice;
using Rytor.Craps.Microservices.Orchestrator.Interfaces;
using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Services
{
    public class GameService : IGameService
    {
        private readonly string _gameURL;
        private readonly ILogger<GameService> _logger;
        private readonly string _className = "GameService";
        private readonly int _phaseInterval;
        private bool _isStarted;
        private PeriodicTimer _timer;
        private RollResult _lastRoll = new RollResult();

        public GameService(int phaseInterval, string gameURL, ILoggerFactory loggerFactory)
        {
            _phaseInterval = phaseInterval;
             _gameURL = gameURL;
             _isStarted = false;
             _logger = loggerFactory.CreateLogger<GameService>();
            _logger.LogInformation("GameService initialized.");
        }

        public async Task<Game> GetGame()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);
            HttpResponseMessage getResponse = await client.GetAsync(String.Concat(_gameURL, "/game"));
            if (getResponse.StatusCode == HttpStatusCode.OK)
            {
                Game result = await getResponse.Content.ReadFromJsonAsync<Game>();
                return result;
            }
            else
                return null;
        }

        public RollResult GetLastDiceRoll()
        {
            return _lastRoll;
        }

        public async Task<List<Bet>> GetBets()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);
            HttpResponseMessage getResponse = await client.GetAsync(String.Concat(_gameURL, "/bet"));
            if (getResponse.StatusCode == HttpStatusCode.OK)
            {
                List<Bet> result = await getResponse.Content.ReadFromJsonAsync<List<Bet>>();
                return result;
            }
            else
                return null;
        }

        public TimeSpan GetTimeLeftInInterval()
        {
            return new TimeSpan(1);
        }

        public bool StartAutomatedGame()
        {
            if (_isStarted)
                return false;
            else
            {
                _isStarted = true;
                RunAutomatedGame();
                return true;
            }
        }

        public bool EndAutomatedGame()
        {
            if (!_isStarted)
                return false;
            else
            {
                _isStarted = false;
                return true;
            }
        }

        private async Task<int> RunAutomatedGame()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Int64 ticks = 0;

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);

            using (_timer = new PeriodicTimer(TimeSpan.FromSeconds(_phaseInterval)))
            {
                _logger.LogInformation("Automated Game Timer Started");
                try
                {
                    while (await _timer.WaitForNextTickAsync(cts.Token))
                    {
                        if (!_isStarted)
                            // EndAutomatedGame must have been called. Stop the timer and end the function
                            cts.Cancel();
                        else
                        {                        
                            if (ticks > 0)
                            {
                                HttpResponseMessage getGameStateResponse = await client.GetAsync(String.Concat(_gameURL, "/game"));
                                if (getGameStateResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    Game game = await getGameStateResponse.Content.ReadFromJsonAsync<Game>();
                                    _logger.LogInformation($"Starting Game State: Phase - {game.State} / Number of Rolls - {game.NumberOfRolls} / Point - {game.Point} / LastGameEvents - {game.LastGameEvents.ToString()} / Completed - {game.Completed}");

                                    if (game.Completed)
                                        ResetGame();
                                    else
                                    {
                                        switch (game.State)
                                        {
                                            case GameState.OpeningRollBets:
                                                _logger.LogInformation("Betting is closed!");
                                                game = await AdvanceGame();
                                                break;
                                            case GameState.OpeningRoll:
                                                _logger.LogInformation("Rolling Dice...");
                                                _lastRoll = await RollDice();
                                                _logger.LogInformation($"Dice has rolled a {_lastRoll.Total} - {_lastRoll.Dice[0].Value}|{_lastRoll.Dice[1].Value}");
                                                game = await HandleDiceRoll(_lastRoll);
                                                _logger.LogInformation($"Closing Game State: Phase - {game.State} / Number of Rolls - {game.NumberOfRolls} / Point - {game.Point} / LastGameEvents - {game.LastGameEvents.ToString()} / Completed - {game.Completed}");
                                                game = await AdvanceGame();
                                                break;
                                            case GameState.SubsequentRollBets:
                                                _logger.LogInformation($"Point is {game.Point}! Betting is closed!");
                                                game = await AdvanceGame();
                                                break;
                                            case GameState.SubsequentRoll:
                                                _logger.LogInformation("Betting is closed!");
                                                _logger.LogInformation("Rolling Dice...");
                                                _lastRoll = await RollDice();
                                                _logger.LogInformation($"Dice has rolled a {_lastRoll.Total} - {_lastRoll.Dice[0].Value}|{_lastRoll.Dice[1].Value}");
                                                game = await HandleDiceRoll(_lastRoll);
                                                _logger.LogInformation($"Closing Game State: Phase - {game.State} / Number of Rolls - {game.NumberOfRolls} / Point - {game.Point} / LastGameEvents - {game.LastGameEvents.ToString()} / Completed - {game.Completed}");
                                                game = await AdvanceGame();
                                                break;
                                            default: break;
                                        }
                                    }
                                }
                            else
                                _logger.LogError("Unable to get Game State.");
                            }
                            else
                            {
                                // First pass - run ResetGame command to start at clean slate
                                ResetGame();
                            }
                            ticks++;
                        }
                    }
                }
                catch (OperationCanceledException ocex)
                {
                    _logger.LogInformation("Automated Game Timer Stopped");
                    return 0;
                }
            }

            return -1;
        }

        private async void ResetGame()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);
            HttpResponseMessage getGameResetResponse = await client.GetAsync(String.Concat(_gameURL, "/game/reset"));
            if (getGameResetResponse.IsSuccessStatusCode)
                    _logger.LogInformation("Game Initialized");
            else
                _logger.LogError("Unable to initialize game.");
        }

        private async Task<RollResult> RollDice()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);
            HttpResponseMessage getDiceRollResponse = await client.GetAsync(String.Concat(_gameURL, "/dice/roll"));
            if (getDiceRollResponse.IsSuccessStatusCode)
            {
                RollResult result = await getDiceRollResponse.Content.ReadFromJsonAsync<RollResult>();
                return result;
            }            
            else
            {
                _logger.LogError("Unable to roll dice.");
                return null;
            }         
        }

        private async Task<Game> HandleDiceRoll(RollResult diceRoll)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);
            HttpResponseMessage getHandleRollResponse = await client.PostAsJsonAsync(String.Concat(_gameURL, "/game/roll"), diceRoll);
            if (getHandleRollResponse.IsSuccessStatusCode)
            {
                Game result = await getHandleRollResponse.Content.ReadFromJsonAsync<Game>();
                return result;
            }            
            else
            {
                _logger.LogError("Unable to roll dice.");
                return null;
            }
        }

        private async Task<Game> AdvanceGame()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);
            HttpResponseMessage getAdvanceGameResponse = await client.GetAsync(String.Concat(_gameURL, "/game/advance"));
            if (getAdvanceGameResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation("Advancing game");
                Game result = await getAdvanceGameResponse.Content.ReadFromJsonAsync<Game>();
                return result;
            }         
            else
            {
                _logger.LogError("Unable to advance game.");
                return null;
            }
                
        }
    }
}
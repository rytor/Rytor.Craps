using System.Net;
using Rytor.Craps.Microservices.Orchestrator.Interfaces;
using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Services
{
    public class GameService : IGameService
    {
        private readonly string _gameURL;
        private readonly ILogger<GameService> _logger;
        private readonly string _className = "GameService";

        public GameService(string gameURL, ILoggerFactory loggerFactory)
        {
             _gameURL = gameURL;
             _logger = loggerFactory.CreateLogger<GameService>();
            _logger.LogInformation("GameService initialized.");
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
                var result = await getResponse.Content.ReadFromJsonAsync<List<Bet>>();
                return result;
            }
            else
                return null;
        }
    }
}
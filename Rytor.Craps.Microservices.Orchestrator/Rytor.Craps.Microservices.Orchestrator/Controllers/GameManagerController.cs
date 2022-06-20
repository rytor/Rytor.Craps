using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rytor.Craps.Microservices.Orchestrator.Interfaces;
using Rytor.Craps.Microservices.Orchestrator.Models;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class GameManagerController : ControllerBase
{
    private readonly ILogger<GameManagerController> _logger;
    private readonly IAccountService _accountService;
    private readonly IGameService _gameService;

    public GameManagerController(IAccountService accountService, IGameService gameService, ILogger<GameManagerController> logger)
    {
        _accountService = accountService;
        _gameService = gameService;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<GameUIState> Get()
    {
        // return game state/bet state for Front-End
        List<Account> _allAccounts = _accountService.GetAccounts().Result;
        List<Bet> _bets = _gameService.GetBets().Result;
        Game _game = _gameService.GetGame().Result;
        RollResult _lastRoll = _gameService.GetLastDiceRoll();

        List<string> _allGameStateStrings = GameState.GetNames(typeof(GameState)).ToList();
        List<string> _allEventStrings = GameEvent.GetNames(typeof(GameEvent)).ToList();


        // reconcile account ids with account usernames for display
        List<BetState> bs = new List<BetState>();
        foreach (Bet b1 in b)
        {
            int locationIndex = (int)b1.GameEventId;
            bs.Add(new BetState {player = a.Where(y => y.Id == b1.AccountId).First().TwitchId, amount = b1.Amount, location = c[locationIndex-1].ToLower()});
        }

        GameUIState mockState = new GameUIState { rolling = false, bets = bs, point = "five" };

        return Ok(mockState);
    }

    [HttpPut("start")]
    public ActionResult<string> StartAutomatedGame()
    {
        bool gameStarted = _gameService.StartAutomatedGame();

        if (gameStarted)
            return Ok();
        else
            return BadRequest();
    }

    [HttpPut("end")]
    public ActionResult<string> EndAutomatedGame()
    {
        bool gameEnded = _gameService.EndAutomatedGame();

        if (gameEnded)
            return Ok();
        else
            return BadRequest();
    }
}

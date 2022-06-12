using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rytor.Craps.Microservices.Orchestrator.Interfaces;
using Rytor.Craps.Microservices.Orchestrator.Models;

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
    public ActionResult<GameState> Get()
    {
        // return game state/bet state for Front-End
        List<Account> a = _accountService.GetAccounts().Result;
        List<Bet> b = _gameService.GetBets().Result;
        List<string> c = GameEvent.GetNames(typeof(GameEvent)).ToList();

        // reconcile account ids with account usernames for display
        List<BetState> bs = new List<BetState>();
        foreach (Bet b1 in b)
        {
            int locationIndex = (int)b1.GameEventId;
            bs.Add(new BetState {player = a.Where(y => y.Id == b1.AccountId).First().TwitchId, amount = b1.Amount, location = c[locationIndex-1].ToLower()});
        }

        GameState mockState = new GameState { rolling = false, bets = bs, point = "five" };

        return Ok(mockState);
    }

    [HttpPost(Name = "advance")]
    public ActionResult<string> RollAndAdvanceGame()
    {
        // roll dice (if needed)

        // send game events to bet service to fulfill bets

        // advance game state

        return Ok(string.Empty);
    }
}

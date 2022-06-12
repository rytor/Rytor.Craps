using Microsoft.AspNetCore.Mvc;
using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class GameManagerController : ControllerBase
{
    private readonly ILogger<GameManagerController> _logger;

    public GameManagerController(ILogger<GameManagerController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<GameState> Get()
    {
        // return game state/bet state for Front-End
        List<Bet> b = new List<Bet>();
        b.Add(new Bet {player = "rytor", amount = 100, location = "six"});
        b.Add(new Bet {player = "max", amount = 300, location = "nine"});
        b.Add(new Bet {player = "jason", amount = 200, location = "eight"});
        GameState mockState = new GameState { rolling = false, bets = b, point = "five" };

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

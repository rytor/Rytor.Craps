using Microsoft.AspNetCore.Mvc;
using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class BetController : ControllerBase
{
    private readonly ILogger<BetController> _logger;

    public BetController(ILogger<BetController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "place")]
    public ActionResult<string> PlaceBet(BetRequest betRequest)
    {
        // check if user exists

        // check if bet already exists (update it)

        // call Balance controller

        // store bet

        return Ok(string.Empty);
    }

    [HttpPost(Name = "cancel")]
    public ActionResult<string> CancelBet(BetRequest betRequest)
    {
        // check if user exists

        // check if bet already exists (cancel it)

        // call Balance controller

        // delete bet

        return Ok(string.Empty);
    }
}

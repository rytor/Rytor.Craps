using Microsoft.AspNetCore.Mvc;
using Rytor.Craps.Microservices.Orchestrator.Interfaces;
using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class BalanceController : ControllerBase
{
    private readonly ILogger<BalanceController> _logger;
    private IBalanceService _balanceService;
    private IAccountService _accountService;

    public BalanceController(IAccountService accountService, IBalanceService balanceService, ILogger<BalanceController> logger)
    {
        _logger = logger;
        _accountService = accountService;
        _balanceService = balanceService;
    }

    [HttpGet("{twitchId}")]
    public ActionResult<Balance> Get(string twitchId)
    {
        int accountId = _accountService.CheckInAccount(twitchId).Result;
        if (accountId > 0)
        {
            Task<Balance> result = _balanceService.GetBalance(accountId);

            return Ok(result.Result);
        }
        else
        {
            return BadRequest();
        }
        
    }
}

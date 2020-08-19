using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models = Rytor.Craps.Microservices.Balance.Models;
using Rytor.Craps.Microservices.Balance.Interfaces;

namespace Rytor.Craps.Microservices.Balance.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly IBalanceRepository _balanceRepository;
        private readonly IActivityRepository _activityRepository;

        public ActivityController(ILoggerFactory loggerFactory, IBalanceRepository balanceRepository, IActivityRepository activityRepository)
        {
            _logger = loggerFactory.CreateLogger<ActivityController>();
            _logger.LogInformation("BalanceController initialized.");
            _balanceRepository = balanceRepository;
            _activityRepository = activityRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Models.Activity>> Get()
        {
            return Ok(_activityRepository.GetActivity());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var result = _activityRepository.GetActivityById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("account/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Models.Activity>> GetByAccountId(int id)
        {
            var result = _activityRepository.GetActivityByAccountId(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] Models.Activity activity)
        {
            var result = _activityRepository.CreateActivity(activity);
            if (result > 0)
            {
                return StatusCode(201, result);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Models.Activity activity)
        {
            Models.Activity result = _activityRepository.UpdateActivity(activity);

            if (result != null)
            {
                return Accepted(result);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _activityRepository.DeleteActivity(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("deposit")]
        public IActionResult Deposit(Models.ActivityRequest request)
        {
            // Make sure amount is greater than zero
            if (request.Amount > 0)
            {
                // Update Balance
                Models.Balance accountBalance = _balanceRepository.GetBalanceById(request.AccountId);
                accountBalance.CurrentBalance += request.Amount;
                Models.Balance result = _balanceRepository.UpdateBalance(accountBalance);

                // Add Activity
                _activityRepository.CreateActivity(new Models.Activity { AccountId = request.AccountId, Amount = request.Amount, ActivityTypeId = Models.ActivityType.Deposit} );

                return Accepted(result);
            }
            else
            {
                return BadRequest("Amount must be greater than zero.");
            }
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw(Models.ActivityRequest request)
        {
            // Make sure amount is greater than zero
            if (request.Amount > 0)
            {
                // Update Balance
                Models.Balance accountBalance = _balanceRepository.GetBalanceById(request.AccountId);
                accountBalance.CurrentBalance -= request.Amount;
                Models.Balance result = _balanceRepository.UpdateBalance(accountBalance);

                // Add Activity
                _activityRepository.CreateActivity(new Models.Activity { AccountId = request.AccountId, Amount = request.Amount, ActivityTypeId = Models.ActivityType.Withdrawal} );
                return Accepted(result);
            }
            else
            {
                return BadRequest("Amount must be greater than zero.");
            }
        }
    }
}

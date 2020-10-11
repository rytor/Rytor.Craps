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
    public class BalanceController : ControllerBase
    {
        private readonly ILogger<BalanceController> _logger;
        private readonly IBalanceRepository _balanceRepository;

        public BalanceController(ILoggerFactory loggerFactory, IBalanceRepository balanceRepository)
        {
            _logger = loggerFactory.CreateLogger<BalanceController>();
            _logger.LogInformation("BalanceController initialized.");
            _balanceRepository = balanceRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Models.Balance>> Get()
        {
            return Ok(_balanceRepository.GetBalances());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var result = _balanceRepository.GetBalanceById(id);
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
        public IActionResult Post([FromBody] Models.Balance balance)
        {
            var result = _balanceRepository.CreateBalance(balance);
            if (result == 0)
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Models.Balance balance)
        {
            Models.Balance result = _balanceRepository.UpdateBalance(balance);

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
            bool result = _balanceRepository.DeleteBalance(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}

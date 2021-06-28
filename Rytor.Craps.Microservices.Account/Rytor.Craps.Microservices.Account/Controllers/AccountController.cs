using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rytor.Craps.Microservices.Account.Repositories;

namespace Rytor.Craps.Microservices.Account.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepository;

        public AccountController(ILoggerFactory loggerFactory, IAccountRepository accountRepository)
        {
            _logger = loggerFactory.CreateLogger<AccountController>();
            _logger.LogInformation("AccountController initialized.");
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Models.Account>> Get()
        {
            return Ok(_accountRepository.GetAccounts());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var result = _accountRepository.GetAccountById(id);
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
        public IActionResult Post([FromBody] Models.Account account)
        {
            var result = _accountRepository.CreateAccount(account);
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
        public IActionResult Put([FromBody] Models.Account account)
        {
            Models.Account result = _accountRepository.UpdateAccount(account);

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
            bool result = _accountRepository.DeleteAccount(id);

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

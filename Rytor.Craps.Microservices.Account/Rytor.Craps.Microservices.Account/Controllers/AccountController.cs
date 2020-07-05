using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models = Rytor.Craps.Microservices.Account.Models;
using Rytor.Craps.Microservices.Account.Interfaces;

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
    }
}

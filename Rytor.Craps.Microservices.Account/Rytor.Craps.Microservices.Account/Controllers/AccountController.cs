using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IEnumerable<Models.Account> Get()
        {
            return _accountRepository.GetAccounts();
        }
    }
}

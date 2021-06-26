using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rytor.Craps.Microservices.Game.Repositories;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BetController : ControllerBase
    {
        private readonly ILogger<BetController> _logger;
        private IGameRepository _gameRepository;

        public BetController(ILoggerFactory loggerFactory, IGameRepository gameRepository)
        {
            _logger = loggerFactory.CreateLogger<BetController>();
            _logger.LogInformation("BetController initialized.");
            _gameRepository = gameRepository;
        }

        [HttpGet]
        [Route("payout")]
        public ActionResult<RollResult> Roll()
        {
            return Ok(_gameRepository.GetGameEventPayouts());
        }
    }
}

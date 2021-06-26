using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rytor.Craps.Microservices.Game.Models;
using Rytor.Craps.Microservices.Game.Repositories;
using Rytor.Craps.Microservices.Game.Services;

namespace Rytor.Craps.Microservices.Game.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BetController : ControllerBase
    {
        private readonly ILogger<BetController> _logger;
        private IGameRepository _gameRepository;
        private IBetService _betService;

        public BetController(ILoggerFactory loggerFactory, IGameRepository gameRepository, IBetService betService)
        {
            _logger = loggerFactory.CreateLogger<BetController>();
            _logger.LogInformation("BetController initialized.");
            _gameRepository = gameRepository;
            _betService = betService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("payout")]
        public ActionResult<IEnumerable<GameEventPayout>> GetPayouts()
        {
            return Ok(_gameRepository.GetGameEventPayouts());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Bet>> Get()
        {
            return Ok(_gameRepository.GetBets());
        }

        [HttpGet("account/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Bet>> GetByAccountId(int id)
        {
            var result = _gameRepository.GetBetsByAccountId(id);
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
        public IActionResult Post([FromBody] Bet bet)
        {
            var result = _gameRepository.CreateBet(bet);
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
        public IActionResult Put([FromBody] Bet bet)
        {
            Bet result = _gameRepository.UpdateBet(bet);

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
            bool result = _gameRepository.DeleteBet(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("handle")]
        public ActionResult<IEnumerable<Bet>> HandleBets([FromBody] HandleBetRequest request)
        {
            return Ok(_betService.HandleBets(request.Game, request.Roll, request.Bets));
        }
    }
}

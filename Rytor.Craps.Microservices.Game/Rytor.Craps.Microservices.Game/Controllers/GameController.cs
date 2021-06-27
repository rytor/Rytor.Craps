using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rytor.Craps.Microservices.Game.Services;
using Rytor.Libraries.Dice;
using Models = Rytor.Craps.Microservices.Game.Models;

namespace Rytor.Craps.Microservices.Game.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private IGameService _gameService;
        private IBetService _betService;

        public GameController(IGameService gameService, IBetService betService)
        {
            _gameService = gameService;
            _betService = betService;
        }

        [HttpGet]
        public ActionResult<Models.Game> Get()
        {
            return Ok(_gameService.GetGame());
        }

        [HttpGet]
        [Route("reset")]
        public ActionResult<Models.Game> ResetGame()
        {
            return Ok(_gameService.ResetGame());
        }

        [HttpGet]
        [Route("advance")]
        public ActionResult<Models.Game> AdvanceGame()
        {
            return Ok(_gameService.AdvanceGameState());
        }

        [HttpPost]
        [Route("roll")]
        public ActionResult<Models.Game> HandleRoll([FromBody] RollResult roll)
        {
            return Ok(_gameService.HandleRoll(roll));
        }
    }
}

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
        private Models.Game _game;
        private IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
            _game = _gameService.ResetGame();
        }

        [HttpGet]
        public ActionResult<Models.Game> Get()
        {
            return Ok(_game);
        }

        [HttpGet]
        [Route("reset")]
        public ActionResult<Models.Game> ResetGame()
        {
            _game = _gameService.ResetGame();
            return Ok(_game);
        }

        [HttpGet]
        [Route("advance")]
        public ActionResult<Models.Game> AdvanceGame()
        {
            _game = _gameService.AdvanceGameState();
            return Ok(_game);
        }

        [HttpPost]
        [Route("roll")]
        public ActionResult<Models.Game> HandleRoll([FromBody] RollResult roll)
        {
            _game = _gameService.HandleRoll(roll);
            return Ok(_game);
        }
    }
}

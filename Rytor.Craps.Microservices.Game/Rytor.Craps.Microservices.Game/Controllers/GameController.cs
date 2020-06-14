using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models = Rytor.Craps.Microservices.Game.Models;

namespace Rytor.Craps.Microservices.Game.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        Models.Game _game;

        public GameController()
        {
            _game = new Models.Game();
        }

        [HttpGet]
        public ActionResult<Models.Game> Get()
        {
            return Ok(_game);
        }
    }
}

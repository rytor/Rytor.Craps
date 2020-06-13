using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiceController : ControllerBase
    {
        [HttpGet]
        public int Get()
        {
            var dice = new Dice(2, 6);
            var result = dice.Roll();
            return result.Total;
        }
    }
}

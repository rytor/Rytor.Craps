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
        Dice _dice;

        public DiceController()
        {
            _dice = new Dice(2, 6);
        }

        [HttpGet]
        [Route("roll")]
        public int Roll()
        {
            return _dice.Roll().Total;
        }
    }
}

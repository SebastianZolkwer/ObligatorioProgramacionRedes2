using BusinessLogicInterface.Interfaces;
using Bussiness.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("games")]
    public class GameController : ControllerBase
    {

        private IGameLogic gameLogic;

        public GameController(IGameLogic _userLogic)
        {
            this.gameLogic = _userLogic;
        }

        [HttpPost]
        public IActionResult Create([FromBodyAttribute] Game game)
        {
            var gameCreated = gameLogic.Create(game);
            return Ok(gameCreated);
        }

        [HttpGet]
        public IActionResult Get([FromBodyAttribute] string title)
        {
            var game = gameLogic.Get(title);
            return Ok(game);
        }

        [HttpPost]
        public IActionResult Update([FromBodyAttribute] Game game)
        {
            var gameUpdated = gameLogic.Update(game);
            return Ok(gameUpdated);
        }

        [HttpDelete]
        public IActionResult Delete([FromBodyAttribute] string title)
        {
            var game = gameLogic.Delete(title);
            return Ok(game);
        }
    }
}

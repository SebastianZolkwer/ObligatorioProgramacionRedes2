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
        public async Task<IActionResult> CreateAsync([FromBody] Game game)
        {
            var gameCreated = await gameLogic.CreateAsync(game);
            return Ok(gameCreated);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string title)
        {
            var game = await gameLogic.GetAsync(title);
            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync([FromQuery] string title, [FromBody] Game game)
        {
            var gameUpdated = await gameLogic.UpdateAsync(title,game);
            return Ok(gameUpdated);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string title)
        {
            var game = await gameLogic.DeleteAsync(title);
            return Ok(game);
        }
    }
}

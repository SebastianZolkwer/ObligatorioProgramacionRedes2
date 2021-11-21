using BusinessLogicInterface.Interfaces;
using Bussiness.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
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
            try
            {
                var gameCreated = await gameLogic.CreateAsync(game);
                var gameToShoW = new GameDto(gameCreated);
                return Ok(gameToShoW);
            }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var games = await gameLogic.GetAllAsync();
                var gamesReturn = games.Select(game => new GameDto(game)).ToList();
                return Ok(gamesReturn);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{title}")]
        public async Task<IActionResult> GetAsync(string title)
        {
            try
            {
                var game = await gameLogic.GetAsync(title);
                var gameToShow = new GameDto(game);
                return Ok(gameToShow);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{title}")]
        public async Task<IActionResult> UpdateAsync(string title, [FromBody] Game game)
        {
            try
            {
                var gameUpdated = await gameLogic.UpdateAsync(title, game);
                var gameToShow = new GameDto(game);
                return Ok(gameUpdated);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{title}")]
        public async Task<IActionResult> Delete(string title)
        {
            try
            {
                var game = await gameLogic.DeleteAsync(title);
                return Ok(game);
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}

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
    [Route("clients")]
    public class ClientController : ControllerBase
    {

        private IClientLogic clientLogic;

        public ClientController(IClientLogic _userLogic)
        {
            this.clientLogic = _userLogic;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] Client user)
        {
            var userCreated = await clientLogic.CreateAsync(user);
            return Ok(userCreated);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await clientLogic.GetAllAsync();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync([FromBody] Client oldClient, [FromBody] Client newClient)
        {
            var userUpdated = await clientLogic.UpdateAsync(oldClient, newClient);
            return Ok(userUpdated);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] Client client)
        {
            var user = await clientLogic.DeleteAsync(client);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> BuyGameAsync([FromBody] UserBuyGame gameBuy)
        {
            var message = await clientLogic.BuyAsync(gameBuy);
            return Ok(message);
        }
    }
}

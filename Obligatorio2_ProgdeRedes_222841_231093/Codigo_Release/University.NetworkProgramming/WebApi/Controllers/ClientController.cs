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
    [Route("clients")]
    public class ClientController : ControllerBase
    {

        private IClientLogic clientLogic;

        public ClientController(IClientLogic _userLogic)
        {
            this.clientLogic = _userLogic;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBodyAttribute] Client user)
        {
            var userCreated = clientLogic.Create(user);
            return Ok(userCreated);
        }

        [HttpGet]
        public IActionResult Get([FromBodyAttribute] string name)
        {
            var user = clientLogic.Get(name);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Update([FromBodyAttribute] Client user)
        {
            var userUpdated = clientLogic.Update(user);
            return Ok(userUpdated);
        }

        [HttpDelete]
        public IActionResult Delete([FromBodyAttribute] string name)
        {
            var user = clientLogic.Delete(name);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult BuyGame([FromBodyAttribute] string gameTitle)
        {
            var message = clientLogic.Buy(gameTitle);
            return Ok(message);
        }
    }
}

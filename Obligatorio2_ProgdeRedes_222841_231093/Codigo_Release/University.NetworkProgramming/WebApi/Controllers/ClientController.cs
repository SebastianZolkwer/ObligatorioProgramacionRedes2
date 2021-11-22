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
        public async Task<IActionResult> CreateUserAsync([FromBody] Client client)
        {
            try
            {
                var userCreated = await clientLogic.CreateAsync(client);
                var userToShow = new ClientDto(userCreated);
                return Ok(userToShow);
            }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpGet]
        public async Task<IActionResult>  GetAsync()
        {
            try { 
                var users = await clientLogic.GetAllAsync();
                var usersReturn = users.Select(user => new ClientDto(user)).ToList();
                return Ok(usersReturn);
            }catch(Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateAsync(string name, [FromBody] Client newClient)
        {try
            {
                var userUpdated = await clientLogic.UpdateAsync(name, newClient);
                return Ok(userUpdated);
            }
            catch (Exception e) { return BadRequest(e.Message); } 
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            try
            {
                var user = await clientLogic.DeleteAsync(name);
                return Ok(user);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{name}/BuyGame")]
        public async Task<IActionResult> BuyGameAsync(string name,[FromQuery] string title)
        {
            try
            {
                var message = await clientLogic.BuyGameAsync(name, title);
                return Ok(message);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{name}/ReturnGame")]
        public async Task<IActionResult> ReturnGameAsync(string name, [FromQuery] string title)
        {
            try
            {
                var message = await clientLogic.ReturnGameAsync(name, title);
                return Ok(message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

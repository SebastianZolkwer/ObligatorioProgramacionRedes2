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
    [Route("users")]
    public class LogController : ControllerBase
    {

        private ILogLogic logLogic;

        public LogController(ILogLogic _userLogic)
        {
            this.logLogic = _userLogic;
        }

        [HttpGet()]
        public IActionResult GetAll([FromQuery] string userName, [FromQuery] string gameTitle, [FromQuery] string date)
        {
            var logs = logLogic.GetAll(userName, gameTitle, date);
            return Ok(logs);
        }
    }
}

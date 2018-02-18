using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
using Apprefine.MattermostBots.PollBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace Apprefine.MattermostBots.PollBot.Controllers
{
    /// <summary>
    /// Answers to slash commands
    /// </summary>
    [Route("api/slash")]
    public class SlashController : Controller
    {
        private readonly SlashCommandSrv _commandSrv;

        public SlashController(SlashCommandSrv commandSrv)
        {
            _commandSrv = commandSrv;
        }

        [HttpPost]
        [Route("")]
        public Task<MattermostResponse> HandleMessage([FromForm]MattermostRequest req)
        {
            return _commandSrv.Handle(req);
        }
    }
}

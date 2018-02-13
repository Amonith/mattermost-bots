using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apprefine.MattermostBots.Common.Models;
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
        private readonly PollSrv _pollSrv;

        public SlashController(PollSrv pollSrv)
        {
            _pollSrv = pollSrv;
        }

        [HttpPost]
        [Route("")]
        public Task<MattermostResponse> HandleMessage([FromForm]MattermostRequest req)
        {
            return _pollSrv.HandleCommand(req);
        }
    }
}

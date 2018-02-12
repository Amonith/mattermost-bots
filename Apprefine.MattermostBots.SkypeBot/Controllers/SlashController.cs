using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkypeBot.Services;
using Microsoft.AspNetCore.Mvc;
using Apprefine.MattermostBots.Common.Models;

namespace SkypeBot.Controllers
{
    /// <summary>
    /// Answers to slash commands
    /// </summary>
    [Route("api/slash")]
    public class SlashController : Controller
    {
        private readonly SlashCommandSrv _slashCommandSrv;

        public SlashController(SlashCommandSrv slashCommandSrv)
        {
            _slashCommandSrv = slashCommandSrv;
        }

        [HttpPost]
        [Route("")]
        public Task<MattermostResponse> HandleMessage([FromForm]MattermostRequest req)
        {
            return _slashCommandSrv.Handle(req);
        }
    }
}

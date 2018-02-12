using Apprefine.MattermostBots.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkypeBot.Services.SlashCommand
{
    public interface IHandler
    {
        Task<MattermostResponse> Handle(MattermostRequest req);
    }
}

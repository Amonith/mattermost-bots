using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
using SkypeBot.Entities;
using SkypeBot.Services.SlashCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkypeBot.Services
{
    public class SlashCommandSrv
    {
        private SkypeBotContext _dbContext;
        private MattermostSrv _mattermostSrv;

        public SlashCommandSrv(
            SkypeBotContext dbContext,
            MattermostSrv mattermostSrv
        )
        {
            _dbContext = dbContext;
            _mattermostSrv = mattermostSrv;
        }

        public Task<MattermostResponse> Handle(MattermostRequest req)
        {
            switch(req.command)
            {
                case "/skype":
                    return new SkypeHandler(_dbContext, _mattermostSrv).Handle(req);
                default:
                    return null;
            }
        }
    }
}

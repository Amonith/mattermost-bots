using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.RandomBot.Services
{
    public class RandomHandler : CommandHandler
    {
        private readonly MattermostSrv _mattermostSrv;

        public RandomHandler(MattermostSrv mattermostSrv)
            : base("/random")
        {
            _mattermostSrv = mattermostSrv;

            Register("member", GetRandomMember);
            Register("number", GetRandomNumber);
            Register("", PrintUsage);
        }

        public Task<MattermostResponse> GetRandomMember(MattermostRequest req)
        {
            //TODO:
            //-get channel members with matermostSrv
            //-pick random guy and send an InChannel message
            throw new NotImplementedException();
        }

        public Task<MattermostResponse> GetRandomNumber(MattermostRequest req)
        {
            //TODO:
            //- /random number range 1 100
            //- /random number from 1 2 3 4 5
            throw new NotImplementedException();
        }

        public Task<MattermostResponse> PrintUsage(MattermostRequest req)
        {
            throw new NotImplementedException();
        }
    }
}

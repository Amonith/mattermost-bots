using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
using Apprefine.MattermostBots.RandomBot.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public async Task<MattermostResponse> GetRandomMember(MattermostRequest req)
        {
            //-get channel members with matermostSrv
            //-pick random guy and send an InChannel message

            var members = await _mattermostSrv.GetChannelMembers(req.channel_id);
            var randomMember = members[new Random().Next(members.Count)];
            var randomMemberDetails = await _mattermostSrv.GetUser(randomMember.UserId);

            return new MattermostResponse()
            {
                ResponseType = Common.Consts.ResponseType.InChannel,
                Text = string.Format(
                    Langs.RandomMemberSelected,
                    req.user_name,
                    randomMemberDetails.username
                )
            };
        }

        public Task<MattermostResponse> GetRandomNumber(MattermostRequest req)
        {
            var numbersRegex = new Regex(@"(\d+).*?(\d+)");
            var match = numbersRegex.Match(req.text);

            if(!match.Success)
            {
                return Task.FromResult(
                    new MattermostResponse()
                    {
                        ResponseType = Common.Consts.ResponseType.Ephemeral,
                        Text = string.Format(
                            Langs.RandomNumberUsage    
                        )
                    }
                );
            }

            var randomFrom = int.Parse(match.Groups[1].Value);
            var randomTo = int.Parse(match.Groups[2].Value)+1; //+1 because .NET random max is exclusive

            var random = new Random().Next(randomFrom, randomTo);

            return Task.FromResult(
                new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.InChannel,
                    Text = string.Format(
                        Langs.RandomNumberSelected,
                        req.user_name,
                        randomFrom,
                        match.Groups[2].Value,
                        random
                    )
                }
            );
        }

        public Task<MattermostResponse> PrintUsage(MattermostRequest req)
        {
            return Task.FromResult(new MattermostResponse()
            {
                ResponseType = Common.Consts.ResponseType.Ephemeral,
                Text = Langs.Usage
            });
        }
    }
}

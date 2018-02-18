using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.PollBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.PollBot.Services
{
    public class PollHandler : CommandHandler
    {
        private readonly PollBotContext _dbContext;

        public PollHandler(
            PollBotContext dbContext
        )
        : base("/skype")
        {
            _dbContext = dbContext;

            Register("new", HandleNewPoll);
            Register("answer", HandlePollAnswer);
            Register("close", HandlePollClose);
            Register("results", HandlePollResults);
            Register("", PrintUsage);
        }

        private Task<MattermostResponse> HandleNewPoll(MattermostRequest req)
        {
            //TODO:
            //-extract poll type from text
            //open looks like this: "open DESCRIPTION" where DESCRIPTION is anything
            //closed looks like this: "closed ANSWERS DESCRIPTION" when answers is semicolon separated list of answers
            throw new NotImplementedException();
        }

        private Task<MattermostResponse> HandlePollAnswer(MattermostRequest req)
        {
            //TODO:
            //-find an active poll on current channel
            //-add or update user answer
            throw new NotImplementedException();
        }

        private Task<MattermostResponse> HandlePollClose(MattermostRequest req)
        {
            //TODO:
            //-if there is an active poll on current channel with OwnerId == req.UserId
            //then close it
            //else notify the user that he's retarded
            throw new NotImplementedException();
        }

        private Task<MattermostResponse> HandlePollResults(MattermostRequest req)
        {
            //TODO
            //-send last (active or not) poll results as an InChannel message
            throw new NotImplementedException();
        }

        private Task<MattermostResponse> PrintUsage(MattermostRequest req)
        {
            return Task.FromResult(new MattermostResponse()
            {
                ResponseType = Common.Consts.ResponseType.Ephemeral,
                Text = "TODO"
            });
        }
    }
}

using Apprefine.MattermostBots.Common.Helpers;
using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.PollBot.Entities;
using Apprefine.MattermostBots.PollBot.Resources;
using Microsoft.EntityFrameworkCore;
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

            var cmdParams = req.text.Split(" ").Select(x => x.Trim()).ToList();
            if(cmdParams.Count < 2)
            {
                return Task.FromResult(new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.NewPollUsage
                });
            }

            var type = cmdParams[1].ToLower();
            if (type.StartsWith("o"))
                return HandleNewOpenPoll(req, cmdParams);
            else if (type.StartsWith("c"))
                return HandleNewClosedPoll(req, cmdParams);
            else
                return Task.FromResult(new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.NewPollUsage
                });
        }
         
        private async Task<MattermostResponse> HandleNewOpenPoll(MattermostRequest req, List<string> cmdParams)
        {
            if (cmdParams.Count < 3)
                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.NewOpenPollUsage
                };

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (_dbContext.Polls.Any(p => p.ChannelId == req.channel_id && p.IsActive))
                    return new MattermostResponse()
                    {
                        ResponseType = Common.Consts.ResponseType.Ephemeral,
                        Text = Langs.ActiveOpenPollExists
                    };

                var poll = new Poll()
                {
                    ChannelId = req.channel_id,
                    CreatedAtUtc = DateTime.UtcNow,
                    IsActive = true,
                    OwnerId = req.user_id,
                    Type = Consts.PollType.Open,
                    Description = req.text.Substring(
                        req.text.IndexOf(cmdParams[1]) + cmdParams[1].Length + 1
                    )
                };

                _dbContext.Polls.Add(poll);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.InChannel,
                    Text = string.Format(
                        Langs.OpenPollCreated,
                        req.user_name,
                        poll.Description,
                        poll.Id
                    )
                };
            }
        }

        private Task<MattermostResponse> HandleNewClosedPoll(MattermostRequest req, List<string> cmdParams)
        {
            return Task.FromResult(new MattermostResponse()
            {
                ResponseType = Common.Consts.ResponseType.Ephemeral,
                Text = Langs.NotImplemented
            });
        }

        private async Task<MattermostResponse> HandlePollAnswer(MattermostRequest req)
        {
            var cmdParams = req.text.Split(" ").Select(x => x.Trim()).ToList();
            if (cmdParams.Count < 2)
            {
                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.AnswerUsage
                };
            }

            //-find an active poll on current channel
            //-add or update user answer

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var poll = _dbContext.Polls.SingleOrDefault(
                    x => x.ChannelId == req.channel_id && x.IsActive
                );

                if (poll == null)
                    return new MattermostResponse()
                    {
                        ResponseType = Common.Consts.ResponseType.Ephemeral,
                        Text = Langs.NoActivePolls
                    };

                var answer = _dbContext.PollAnswers.SingleOrDefault(
                    x => x.UserId == req.user_id
                    && x.PollId == poll.Id
                );
                if(answer != null)
                {
                    _dbContext.Remove(answer);
                }

                answer = new PollAnswer()
                {
                    UserId = req.user_id,
                    UserName = req.user_name,
                    PollId = poll.Id,
                    //everything except "answer" - first param
                    Answer = req.text.Substring(
                        req.text.IndexOf(cmdParams[0]) + cmdParams[0].Length + 1
                    )
                };

                _dbContext.PollAnswers.Add(answer);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = string.Format(Langs.AnswerUpdated, poll.Id)
                };
            }
        }

        private async Task<MattermostResponse> HandlePollClose(MattermostRequest req)
        {
            //-if there is an active poll on current channel with OwnerId == req.UserId
            //then close it
            //else notify the user that he's retarded

            var poll = _dbContext.Polls.SingleOrDefault(x => x.OwnerId == req.user_id && x.IsActive);
            if(poll == null)
            {
                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.NoActivePollsByYou
                };
            }
            else
            {
                poll.IsActive = false;
                await _dbContext.SaveChangesAsync();

                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.InChannel,
                    Text = string.Format(
                        Langs.OpenPollClosed,
                        req.user_name,
                        poll.Id
                    )
                };
            }
        }


        /// <summary>
        /// /poll results [ID]
        /// </summary>
        private async Task<MattermostResponse> HandlePollResults(MattermostRequest req)
        {
            //-send poll results as an ephemeral message

            var cmdParams = req.text.Split(" ").Select(x => x.Trim()).ToList();
            int pollId = 0;
            if (cmdParams.Count > 1)
            {
                //get specific poll answers
                var pollIdParam = cmdParams[1];
                if (!int.TryParse(pollIdParam, out pollId))
                {
                    return new MattermostResponse()
                    {
                        ResponseType = Common.Consts.ResponseType.Ephemeral,
                        Text = Langs.ResultsUsage
                    };
                }
            }
            else
            {
                //get latest poll answers
                var latestPoll = await _dbContext.Polls
                    .Where(x => x.ChannelId == req.channel_id)
                    .OrderByDescending(x => x.CreatedAtUtc)
                    .FirstOrDefaultAsync();

                if(latestPoll == null)
                    return new MattermostResponse()
                    {
                        ResponseType = Common.Consts.ResponseType.Ephemeral,
                        Text = Langs.ResultsUsage
                    };

                pollId = latestPoll.Id;
            }

            var answers = await (
                from poll in _dbContext.Polls
                where poll.ChannelId == req.channel_id && poll.Id == pollId
                from pollAnswer in _dbContext.PollAnswers.Where(x => x.PollId == poll.Id).DefaultIfEmpty()
                select pollAnswer
            ).ToListAsync();

            if(answers.Any())
            {
                var table = new TableBuilder();
                foreach (var answer in answers)
                {
                    table
                        .AddColumn("User", answer.UserName)
                        .AddColumn("Answer", answer.Answer);
                }

                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = table.ToString()
                };
            }
            else
            {
                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.NoAnswersFound
                };
            }
        }

        private Task<MattermostResponse> PrintUsage(MattermostRequest req)
        {
            return Task.FromResult(new MattermostResponse()
            {
                ResponseType = Common.Consts.ResponseType.Ephemeral,
                Text = Langs.Usage
            });
        }
    }
}

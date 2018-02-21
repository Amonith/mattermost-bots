using Apprefine.MattermostBots.Common.Helpers;
using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
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
        private readonly MattermostSrv _mattermostSrv;

        public PollHandler(
            PollBotContext dbContext,
            MattermostSrv mattermostSrv
        )
        : base("/skype")
        {
            _dbContext = dbContext;
            _mattermostSrv = mattermostSrv;

            Register("new", HandleNewPoll);
            Register("answer", HandlePollAnswer);
            Register("close", HandlePollClose);
            Register("results", HandlePollResults);
            Register("answer_id", HandleAnswerId);
            Register("list", HandleList);
            Register("reopen", HandleReopen);
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
                var poll = await _dbContext.Polls.Where(
                        x => x.ChannelId == req.channel_id && x.IsActive
                    )
                    .OrderByDescending(x => x.CreatedAtUtc)
                    .FirstOrDefaultAsync();

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

                if(answer == null)
                {
                    answer = new PollAnswer()
                    {
                        UserId = req.user_id,
                        UserName = req.user_name,
                        PollId = poll.Id
                    };

                    _dbContext.PollAnswers.Add(answer);
                }

                //everything except "answer" - first param
                answer.Answer = req.text.Substring(
                    req.text.IndexOf(cmdParams[0]) + cmdParams[0].Length + 1
                );

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
            var cmdParams = req.text.Split(" ").ToList();

            int pollId = 0;
            if(cmdParams.Count > 1)
            {
                if(!int.TryParse(cmdParams[1], out pollId))
                {
                    //TODO: return usage info
                }
            }

            var poll = pollId > 0
                ? _dbContext.Polls
                    .SingleOrDefault(x => x.OwnerId == req.user_id && x.ChannelId == req.channel_id && x.IsActive && x.Id == pollId)
                : _dbContext.Polls
                    .Where(x => x.OwnerId == req.user_id && x.ChannelId == req.channel_id && x.IsActive)
                    .OrderByDescending(x => x.CreatedAtUtc)
                    .FirstOrDefault();

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
                        poll.Id,
                        poll.Description
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

            if(answers != null && answers.Any())
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

        private async Task<MattermostResponse> HandleAnswerId(MattermostRequest req)
        {
            var cmdParams = req.text.Split(" ").ToList();
            if(cmdParams.Count < 3)
            {
                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.AnswerIdUsage
                };
            }

            int pollId = 0;
            if(!int.TryParse(cmdParams[1], out pollId))
            {
                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.AnswerIdUsage
                };
            }

            var poll = await _dbContext.Polls.SingleOrDefaultAsync(x => 
                x.Id == pollId 
                && x.IsActive 
                && x.ChannelId == req.channel_id
            );

            if(poll == null)
            {
                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.ActivePollNotFound
                };
            }

            var answer = _dbContext.PollAnswers.SingleOrDefault(
                x => x.UserId == req.user_id
                && x.PollId == poll.Id
            );

            if (answer == null)
            {
                answer = new PollAnswer()
                {
                    UserId = req.user_id,
                    UserName = req.user_name,
                    PollId = poll.Id
                };

                _dbContext.PollAnswers.Add(answer);
            }

            //everything except "id" - first param
            answer.Answer = req.text.Substring(
                req.text.IndexOf(cmdParams[1]) + cmdParams[1].Length + 1
            );

            await _dbContext.SaveChangesAsync();

            return new MattermostResponse()
            {
                ResponseType = Common.Consts.ResponseType.Ephemeral,
                Text = string.Format(Langs.AnswerUpdated, poll.Id)
            };
        }

        private async Task<MattermostResponse> HandleList(MattermostRequest req)
        {
            var polls = await _dbContext.Polls
                .Where(p => p.ChannelId == req.channel_id)
                .OrderByDescending(p => p.CreatedAtUtc)
                .ToListAsync();

            if(!polls.Any())
            {
                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.NoPollsOnThisChannel
                };
            }

            var ownerNames = await _mattermostSrv.GetUsersByIds(polls.Select(x => x.OwnerId).ToList());
            var ownerNamesDictionary = ownerNames.ToDictionary(x => x.id, x => x.username);

            var table = new TableBuilder();
            foreach(var poll in polls)
            {
                table
                    .AddColumn("Id", poll.Id.ToString())
                    .AddColumn("Description", poll.Description)
                    .AddColumn("Created at", poll.CreatedAtUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm"))
                    .AddColumn("Active", poll.IsActive ? "Yes" : "No")
                    .AddColumn(
                        "Created by", 
                        ownerNamesDictionary.ContainsKey(poll.OwnerId) 
                            ? ownerNamesDictionary[poll.OwnerId]
                            : "*Deleted*"
                    );
            }

            return new MattermostResponse()
            {
                ResponseType = Common.Consts.ResponseType.Ephemeral,
                Text = string.Format(
                    Langs.PollList,
                    table.ToString()
                )
            };
        }

        private async Task<MattermostResponse> HandleReopen(MattermostRequest req)
        {
            var cmdParams = req.text.Split(" ").ToList();
            int id = 0;

            if(cmdParams.Count > 1)
            {
                if(!int.TryParse(cmdParams[1], out id))
                {
                    return new MattermostResponse()
                    {
                        ResponseType = Common.Consts.ResponseType.Ephemeral,
                        Text = Langs.ReopenUsage
                    };
                }
            }

            var pollToReopen = id > 0
                ? await _dbContext.Polls.SingleOrDefaultAsync(p => p.OwnerId == req.user_id && p.Id == id && !p.IsActive)
                : await _dbContext.Polls.Where(p => p.OwnerId == req.user_id && !p.IsActive).OrderByDescending(p => p.CreatedAtUtc).FirstOrDefaultAsync();

            if(pollToReopen == null)
            {
                return new MattermostResponse()
                {
                    ResponseType = Common.Consts.ResponseType.Ephemeral,
                    Text = Langs.NoPollToReopen
                };
            }

            pollToReopen.IsActive = true;
            await _dbContext.SaveChangesAsync();
            return new MattermostResponse()
            {
                ResponseType = Common.Consts.ResponseType.InChannel,
                Text = string.Format(
                    Langs.UserReopenedPoll,
                    req.user_name,
                    pollToReopen.Description,
                    pollToReopen.Id
                )
            };
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

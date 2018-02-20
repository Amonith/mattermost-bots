using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apprefine.MattermostBots.Common;
using Apprefine.MattermostBots.Common.Consts;
using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
using Apprefine.MattermostBots.SkypeBot.Resources;
using SkypeBot.Entities;

namespace SkypeBot.Services
{
    public class SkypeHandler : CommandHandler
    {
        private SkypeBotContext _dbContext;
        private MattermostSrv _matermostSrv;

        public SkypeHandler(
            SkypeBotContext dbContext,
            MattermostSrv mattermostSrv
        )
        : base("/skype")
        {
            _dbContext = dbContext;
            _matermostSrv = mattermostSrv;

            Register("id", SaveId);
            Register("meeting", GenerateMeetingLink);
            Register("join", JoinGroup);
            Register("leave", LeaveGroup);
            Register("", PrintUsage);
        }

        private async Task<MattermostResponse> SaveId(MattermostRequest req)
        {
            var skypeSid = req.text?.Split(" ").ElementAtOrDefault(1);
            if(string.IsNullOrWhiteSpace(skypeSid))
            {
                return new MattermostResponse()
                {
                    ResponseType = ResponseType.Ephemeral,
                    Text = Langs.SaveIdUsage
                };
            }

            var userInfo = _dbContext.UserInfos.SingleOrDefault(x => x.UserId == req.user_id);
            if (userInfo != null)
            {
                if(userInfo.SkypeSID == skypeSid)
                {
                    return new MattermostResponse()
                    {
                        ResponseType = ResponseType.Ephemeral,
                        Text = Langs.IdAlreadySaved
                    };
                }

                _dbContext.UserInfos.Remove(userInfo);
            }

            userInfo = new UserInfo()
            {
                UserId = req.user_id,
                SkypeSID = skypeSid
            };
            _dbContext.Add(userInfo);

            await _dbContext.SaveChangesAsync();

            return new MattermostResponse()
            {
                ResponseType = ResponseType.Ephemeral,
                Text = Langs.IdSaved
            };
        }

        private async Task<MattermostResponse> GenerateMeetingLink(MattermostRequest req)
        {
            var cmdParams = req.text.Split(" ").Select(x => x.Trim()).ToList();
            var group = cmdParams.Count > 1 ? cmdParams[1] : null;

            var channelMembers = await _matermostSrv.GetChannelMembers(req.channel_id);

            var idsExceptCurrent = channelMembers
                .Where(x => x.UserId != req.user_id)
                .Select(m => m.UserId)
                .ToList();

            var userInfoQuery = _dbContext.UserInfos
                .Where(x => idsExceptCurrent.Contains(x.UserId));

            if(group != null)
            {
                userInfoQuery = from userInfo in userInfoQuery
                                from userGroup in _dbContext.UserGroups.Where(x => x.UserId == userInfo.UserId).DefaultIfEmpty()
                                from groupInfo in _dbContext.Groups.Where(x => x.Id == userGroup.GroupId).DefaultIfEmpty()
                                where groupInfo.Name.ToLower() == @group.ToLower()
                                select userInfo;
            }

            var sids = userInfoQuery
                .Select(x => x.SkypeSID);

            if(!sids.Any())
            {
                return new MattermostResponse()
                {
                    ResponseType = ResponseType.Ephemeral,
                    Text = Langs.NoSidsInChannel
                };
            }

            string meetingUrl = "im:" + string.Join(
                "",
                sids.Select(sid => $"<sip:{sid}>")
            );

            var response = new MattermostResponse()
            {
                ResponseType = ResponseType.Ephemeral,
                Text = $"[{Langs.ClickHereToStartMeeting}]({meetingUrl.Replace("<", "&lt;").Replace(">", "&gt;")})",
                GotoLocation = meetingUrl
            };

            if(channelMembers.Count != sids.Count() + 1)
            {
                response.Text += $"\n{Langs.Warning}: {Langs.SomeUsersDontHaveSids} {Langs.SaveIdUsage}";
            }

            return response;
        }

        private async Task<MattermostResponse> JoinGroup(MattermostRequest req)
        {
            var joinParams = req.text.Split(" ").Select(x => x.Trim()).ToList();
            if(joinParams.Count < 2)
            {
                return new MattermostResponse()
                {
                    ResponseType = ResponseType.Ephemeral,
                    Text = Langs.JoinGroupUsage
                };
            }

            var groupNameLower = joinParams[1].ToLower();
            var group = _dbContext.Groups.FirstOrDefault(x => x.Name.ToLower() == groupNameLower);
            if(group == null)
            {
                group = new Apprefine.MattermostBots.SkypeBot.Entities.Group()
                {
                    ChannelId = req.channel_id,
                    Name = groupNameLower
                };
                _dbContext.Groups.Add(group);
                await _dbContext.SaveChangesAsync();
            }

            _dbContext.UserGroups.Add(new Apprefine.MattermostBots.SkypeBot.Entities.UserGroup()
            {
                UserId = req.user_id,
                GroupId = group.Id
            });
            await _dbContext.SaveChangesAsync();

            return new MattermostResponse()
            {
                ResponseType = ResponseType.InChannel,
                Text = string.Format(Langs.UserJoinedGroup, req.user_name, groupNameLower)
            };
        }

        private async Task<MattermostResponse> LeaveGroup(MattermostRequest req)
        {
            var leaveParams = req.text.Split(" ").Select(x => x.Trim()).ToList();
            if (leaveParams.Count < 2)
            {
                return new MattermostResponse()
                {
                    ResponseType = ResponseType.Ephemeral,
                    Text = Langs.LeaveGroupUsage
                };
            }

            var groupNameLower = leaveParams[1].ToLower();
            var userGroupEntry = (
                from userGroup in _dbContext.UserGroups.Where(x => x.UserId == req.user_id)
                from groupInfo in _dbContext.Groups.Where(x => x.Id == userGroup.GroupId && x.Name.ToLower() == groupNameLower)
                select userGroup
            ).SingleOrDefault();

            if (userGroupEntry != null)
            {
                _dbContext.Remove(userGroupEntry);
                await _dbContext.SaveChangesAsync();
            }

            return new MattermostResponse()
            {
                ResponseType = ResponseType.InChannel,
                Text = string.Format(Langs.UserLeftGroup, req.user_id, groupNameLower)
            };
        }

        private Task<MattermostResponse> PrintUsage(MattermostRequest req)
        {
            return Task.FromResult(new MattermostResponse()
            {
                ResponseType = ResponseType.Ephemeral,
                Text = Langs.SkypeCommandUsage
            });
        }
    }
}

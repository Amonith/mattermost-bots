﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apprefine.MattermostBots.Common;
using Apprefine.MattermostBots.Common.Consts;
using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
using Apprefine.MattermostBots.SkypeBot.Resources;
using SkypeBot.Entities;

namespace SkypeBot.Services.SlashCommand
{
    public class SkypeHandler : IHandler
    {
        private SkypeBotContext _dbContext;
        private MattermostSrv _matermostSrv;

        public SkypeHandler(
            SkypeBotContext dbContext,
            MattermostSrv mattermostSrv
        )
        {
            _dbContext = dbContext;
            _matermostSrv = mattermostSrv;
        }

        public async Task<MattermostResponse> Handle(MattermostRequest req)
        {
            var command = req.text?.Split(" ")[0].ToLower();
            switch (command)
            {
                case "id":
                    return await SaveId(req);

                case "meeting":
                    return await GenerateMeetingLink(req);

                default:
                    return PrintUsage(req);
            }

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
            var channelMembers = await _matermostSrv.GetChannelMembers(req.channel_id);

            var idsExceptCurrent = channelMembers
                .Where(x => x.UserId != req.user_id)
                .Select(m => m.UserId)
                .ToList();

            var sids = _dbContext.UserInfos
                .Where(x => idsExceptCurrent.Contains(x.UserId))
                .Select(x => x.SkypeSID);

            string meetingUrl = "im:" + string.Join(
                "",
                sids.Select(sid => $"&lt;sid:{sid}&gt;")
            );

            var response = new MattermostResponse()
            {
                ResponseType = ResponseType.Ephemeral,
                Text = $"[{Langs.ClickHereToStartMeeting}]({meetingUrl})",
                GotoLocation = meetingUrl
            };

            if(channelMembers.Count != sids.Count() + 1)
            {
                response.Text += $"\n{Langs.Warning}: {Langs.SomeUsersDontHaveSids} {Langs.SaveIdUsage}";
            }

            return response;
        }

        private MattermostResponse PrintUsage(MattermostRequest req)
        {
            return new MattermostResponse()
            {
                ResponseType = ResponseType.Ephemeral,
                Text = Langs.SkypeCommandUsage
            };
        }
    }
}

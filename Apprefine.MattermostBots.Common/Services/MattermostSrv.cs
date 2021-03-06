﻿using Apprefine.MattermostBots.Common.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apprefine.MattermostBots.Common.Services
{
    public class MattermostSrv
    {
        private readonly IOptions<MattermostOptions> _options;

        public MattermostSrv(IOptions<MattermostOptions> options)
        {
            _options = options;
        }

        public async Task<List<ChannelMember>> GetChannelMembers(string channelId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.Value.ApiUrl);
                client.DefaultRequestHeaders.Authorization 
                    = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.Value.Token);

                var response = await client.GetAsync($"api/v4/channels/{channelId}/members");
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Could not get members of channel {channelId}. Ensure that correct API token is set in bot's config.");

                return JsonConvert.DeserializeObject<List<ChannelMember>>(
                    await response.Content.ReadAsStringAsync()
                );
            }
        }

        public async Task<User> GetUser(string userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.Value.ApiUrl);
                client.DefaultRequestHeaders.Authorization
                    = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.Value.Token);

                var response = await client.GetAsync($"api/v4/users/{userId}");
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Could not user {userId}. Ensure that correct API token is set in bot's config.");

                return JsonConvert.DeserializeObject<User>(
                    await response.Content.ReadAsStringAsync()
                );
            }
        }

        public async Task<List<User>> GetUsersByIds(IEnumerable<string> ids)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.Value.ApiUrl);
                client.DefaultRequestHeaders.Authorization
                    = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.Value.Token);

                var response = await client.PostAsync($"api/v4/users/ids", new JsonContent(ids));
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Could not get users {string.Join(",", ids)}. Ensure that correct API token is set in bot's config.");

                return JsonConvert.DeserializeObject<List<User>>(
                    await response.Content.ReadAsStringAsync()
                );
            }
        }
    }
}

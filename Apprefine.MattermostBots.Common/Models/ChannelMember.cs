using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.Common.Models
{

    public class ChannelMember
    {
        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("roles")]
        public string Roles { get; set; }

        [JsonProperty("last_viewed_at")]
        public long LastViewedAt { get; set; }

        [JsonProperty("msg_count")]
        public int MsgCount { get; set; }

        [JsonProperty("mention_count")]
        public int MentionCount { get; set; }

        [JsonProperty("notify_props")]
        public NotificationProperties NotifyProps { get; set; }

        [JsonProperty("last_update_at")]
        public long LastUpdateAt { get; set; }
    }
}

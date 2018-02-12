using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apprefine.MattermostBots.Common.Models
{
    public class NotificationProperties
    {
        [JsonProperty("desktop")]
        public string Desktop { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mark_unread")]
        public string MarkUnread { get; set; }

        [JsonProperty("push")]
        public string Push { get; set; }
    }
}

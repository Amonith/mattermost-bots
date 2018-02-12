using Apprefine.MattermostBots.Common.Consts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.Common.Models
{
    public class MattermostResponse
    {
        /// <summary>
        /// If set, the bot will try to override the user name in chat.
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonIgnore]
        public ResponseType ResponseType { get; set; }

        [JsonProperty("response_type")]
        private string ResponseTypeString
        {
            get => ResponseType.ToMattermostValue();
        }

        [JsonProperty("goto_location")]
        public string GotoLocation { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

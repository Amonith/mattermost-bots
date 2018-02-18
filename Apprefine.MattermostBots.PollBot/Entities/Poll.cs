using Apprefine.MattermostBots.PollBot.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.PollBot.Entities
{
    public class Poll
    {
        public int Id { get; set; }
        public PollType Type { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public string ChannelId { get; set; }

        [MaxLength(4000)]
        public string AvailableAnswers { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public bool IsActive { get; set; }
    }
}

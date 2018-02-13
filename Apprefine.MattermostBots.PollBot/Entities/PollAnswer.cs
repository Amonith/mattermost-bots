using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.PollBot.Entities
{
    public class PollAnswer
    {
        public int PollId { get; set; }
        public string UserId { get; set; }

        [MaxLength(4000)]
        public string Answer { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.SkypeBot.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string ChannelId { get; set; }
        public string Name { get; set; }
    }
}

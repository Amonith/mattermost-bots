using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.SkypeBot.Entities
{
    public class UserGroup
    {
        public string UserId { get; set; }
        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}

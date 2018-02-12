using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkypeBot.Entities
{
    /// <summary>
    /// Additional stuff to store alongside user unique id
    /// </summary>
    public class UserInfo
    {
        public string UserId { get; set; }
        public string SkypeSID { get; set; }
    }
}

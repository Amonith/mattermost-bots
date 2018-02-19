using System;
using System.Collections.Generic;
using System.Text;

namespace Apprefine.MattermostBots.Common.Models
{
    /// <summary>
    /// User model from mattermost API
    /// </summary>
    public class User
    {
        public string id { get; set; }
        public long create_at { get; set; }
        public long update_at { get; set; }
        public int delete_at { get; set; }
        public string username { get; set; }
        public string auth_data { get; set; }
        public string auth_service { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string position { get; set; }
        public string roles { get; set; }
        public string locale { get; set; }
    }
}

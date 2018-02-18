using System;
using System.Collections.Generic;
using System.Text;

namespace Apprefine.MattermostBots.Common.Models
{
    public class CommandHandlerFactory
    {
        public string Command { get; set; }
        public Func<CommandHandler> Factory { get; set; }
    }
}

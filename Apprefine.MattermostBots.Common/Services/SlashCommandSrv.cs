using Apprefine.MattermostBots.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.Common.Services
{
    public class SlashCommandSrv
    {
        private Dictionary<string, CommandHandlerFactory> _commandFactories;

        public SlashCommandSrv(
            IEnumerable<CommandHandlerFactory> commandHandlers
        )
        {
            _commandFactories = commandHandlers
                .ToDictionary(x => x.Command, x => x);
        }

        public async Task<MattermostResponse> Handle(MattermostRequest req)
        {
            if(_commandFactories.ContainsKey(req.command))
            {
                return await _commandFactories[req.command].Factory().Handle(req);
            }
            else
            {
                return new MattermostResponse()
                {
                    ResponseType = Consts.ResponseType.Ephemeral,
                    Text = "Command not found. This bot supports the following commands.\n"
                        + string.Join("\n", _commandFactories.Keys.Select(k => "- " + k))
                };
            }
        }
    }
}

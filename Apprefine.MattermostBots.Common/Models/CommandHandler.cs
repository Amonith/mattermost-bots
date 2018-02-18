using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.Common.Models
{
    public class CommandHandler
    {
        public string Command { get; set; }

        private Dictionary<string, Func<MattermostRequest, Task<MattermostResponse>>> _subCommandHandlers;

        public CommandHandler(
            string command, 
            Dictionary<string, Func<MattermostRequest, Task<MattermostResponse>>> subCommandHandlers = null
        )
        {
            if (subCommandHandlers != null)
                _subCommandHandlers = subCommandHandlers;
            else
                _subCommandHandlers = new Dictionary<string, Func<MattermostRequest, Task<MattermostResponse>>>();
        }

        public void Register(string command, Func<MattermostRequest, Task<MattermostResponse>> handleFunc)
        {
            var commandLower = command.ToLower();

            if (_subCommandHandlers.ContainsKey(commandLower))
                throw new ArgumentException("Command already registered.", nameof(command));

            _subCommandHandlers[commandLower] = handleFunc;
        }

        public async Task<MattermostResponse> Handle(MattermostRequest request)
        {
            var subCommand = request.text?.Split(' ')[0]?.Trim().ToLower();
            if (subCommand == null) subCommand = string.Empty;

            if (_subCommandHandlers.ContainsKey(subCommand))
            {
                return await _subCommandHandlers[subCommand](request);
            }
            else
            {
                //TODO: something more than startsWith in the future?
                var similarCommands = _subCommandHandlers.Keys.Where(k => k.StartsWith(subCommand)).ToList();
                if(similarCommands.Count == 1)
                {
                    //e.g.: if user types "/skype j" and skype handler has only 
                    //one command starting with j - "join" then exeute it right away
                    return await _subCommandHandlers[similarCommands[0]](request);
                }
                else
                {
                    return new MattermostResponse()
                    {
                        ResponseType = Consts.ResponseType.Ephemeral,
                        Text = "Multiple commands found:\n"
                            + string.Join("\n", similarCommands.Select(c => "- " + request.command + " " + c))
                    };
                }
            }
        }
    }
}

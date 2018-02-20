using Apprefine.MattermostBots.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.AllBots.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddSlashCommand<THandler>(this IServiceCollection services, string command) where THandler: CommandHandler
        {
            services.AddTransient<THandler>();

            services.AddTransient<CommandHandlerFactory>(
                provider => new CommandHandlerFactory()
                {
                    Command = command,
                    Factory = () => provider.GetService<THandler>()
                }
            );
        }
    }
}

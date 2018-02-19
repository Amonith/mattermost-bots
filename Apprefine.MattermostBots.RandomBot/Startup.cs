using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
using Apprefine.MattermostBots.RandomBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Apprefine.MattermostBots.RandomBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //common for all bots
            services.Configure<MattermostOptions>(Configuration.GetSection("Mattermost"));
            services.AddMvc();
            services.AddTransient<MattermostSrv>();
            services.AddTransient<SlashCommandSrv>(
                provider => new SlashCommandSrv(provider.GetServices<CommandHandlerFactory>())
            );

            //register command handlers
            services.AddTransient<RandomHandler>();

            //register handler factories
            services.AddTransient<CommandHandlerFactory>(
                provider => new CommandHandlerFactory()
                {
                    Command = "/random",
                    Factory = () => provider.GetService<RandomHandler>()
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

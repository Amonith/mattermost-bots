using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
using Apprefine.MattermostBots.PollBot.Entities;
using Apprefine.MattermostBots.PollBot.Filters;
using Apprefine.MattermostBots.PollBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkypeBot.Entities;
using SkypeBot.Services;

namespace Apprefine.MattermostBots.AllBots
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
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.Configure<MattermostOptions>(Configuration.GetSection("Mattermost"));
            services.AddMvc(
                config => {
                    config.Filters.Add(typeof(ExceptionFilter));
                }
            );
            services.AddTransient<MattermostSrv>();
            services.AddTransient<SlashCommandSrv>(
                provider => new SlashCommandSrv(provider.GetServices<CommandHandlerFactory>())
            );

            //register dbcontexts
            services.AddDbContext<PollBotContext>(options =>
                options.UseNpgsql(connectionString)
            );
            services.AddDbContext<SkypeBotContext>(options =>
                options.UseNpgsql(connectionString)
            );

            //register command handlers
            services.AddTransient<SkypeHandler>();
            services.AddTransient<PollHandler>();

            //register handler factories
            services.AddTransient<CommandHandlerFactory>(
                provider => new CommandHandlerFactory()
                {
                    Command = "/poll",
                    Factory = () => provider.GetService<PollHandler>()
                }
            );
            services.AddTransient<CommandHandlerFactory>(
                provider => new CommandHandlerFactory()
                {
                    Command = "/skype",
                    Factory = () => provider.GetService<SkypeHandler>()
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

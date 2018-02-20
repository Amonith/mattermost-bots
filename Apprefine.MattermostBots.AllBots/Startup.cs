using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;
using Apprefine.MattermostBots.PollBot.Entities;
using Apprefine.MattermostBots.PollBot.Filters;
using Apprefine.MattermostBots.PollBot.Services;
using Apprefine.MattermostBots.RandomBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkypeBot.Entities;
using SkypeBot.Services;
using Apprefine.MattermostBots.AllBots.Extensions;

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
            string pollBotConnection = Configuration.GetConnectionString("PollBot");
            services.AddDbContext<PollBotContext>(options =>
                options.UseNpgsql(pollBotConnection)
            );

            string skypeBotConnection = Configuration.GetConnectionString("SkypeBot");
            services.AddDbContext<SkypeBotContext>(options =>
                options.UseNpgsql(skypeBotConnection)
            );

            //register command handlers
            services.AddSlashCommand<SkypeHandler>("/skype");
            services.AddSlashCommand<PollHandler>("/poll");
            services.AddSlashCommand<RandomHandler>("/random");
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

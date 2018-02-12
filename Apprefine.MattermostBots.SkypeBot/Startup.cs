using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkypeBot.Entities;
using SkypeBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Apprefine.MattermostBots.Common.Models;
using Apprefine.MattermostBots.Common.Services;

namespace SkypeBot
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
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.Configure<MattermostOptions>(Configuration.GetSection("Mattermost"));

            services.AddMvc();

            services.AddDbContext<SkypeBotContext>(options =>
                options.UseNpgsql(connectionString)
            );

            services.AddTransient<MattermostSrv>();
            services.AddTransient<SlashCommandSrv>();
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

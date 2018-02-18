using Apprefine.MattermostBots.SkypeBot.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkypeBot.Entities
{
    public class SkypeBotContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        public SkypeBotContext()
        {

        }

        public SkypeBotContext(DbContextOptions<SkypeBotContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("skype");

            modelBuilder.Entity<UserInfo>()
                .HasKey(x => new { x.UserId, x.SkypeSID });

            modelBuilder.Entity<UserGroup>()
                .HasKey(x => new { x.GroupId, x.UserId });
        }
    }
}

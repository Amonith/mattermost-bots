using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apprefine.MattermostBots.PollBot.Entities
{
    public class PollBotContext : DbContext
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollAnswer> PollAnswers { get; set; }

        public PollBotContext()
        {

        }

        public PollBotContext(DbContextOptions<PollBotContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PollAnswer>()
                .HasKey(x => new { x.PollId, x.UserId });
        }
    }
}

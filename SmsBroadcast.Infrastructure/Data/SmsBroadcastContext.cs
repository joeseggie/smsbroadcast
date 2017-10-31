using Microsoft.EntityFrameworkCore;
using SmsBroadcast.Infrastructure.Entities;

namespace SmsBroadcast.Infrastructure.Data
{
    public class SmsBroadcastContext : DbContext
    {
        public SmsBroadcastContext(DbContextOptions<SmsBroadcastContext> options):base(options){}

        public virtual DbSet<ScheduledBroadcast> ScScheduledBroadcasts { get; set; }
        public virtual DbSet<RunOnceBroadcast> RunOnceBroadcasts { get; set; }        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
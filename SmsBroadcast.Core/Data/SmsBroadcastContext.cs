using Microsoft.EntityFrameworkCore;
using SmsBroadcast.Core.Entities;

namespace SmsBroadcast.Core.Data
{
    public class SmsBroadcastContext : DbContext
    {
        public SmsBroadcastContext(DbContextOptions<SmsBroadcastContext> options) : base(options)
        {
        }

        public virtual DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
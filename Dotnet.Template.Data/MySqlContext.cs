using Dotnet.Template.Domain.ActivityLogs;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Template.Data
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<ActivityLog>()
                .Property(p => p.ObjectRef)
                .HasConversion(p => p.ToString(), q => q);
        }
    }
}

using Dotnet.Template.Domain.ActivityLogs;
using Dotnet.Template.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Template.Data
{
    public class MySqlContext(DbContextOptions<MySqlContext> options) : DbContext(options)
    {
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<ActivityLog>()
                .Property(p => p.ObjectRef)
                .HasConversion(p => p.ToString(), q => q);

            modelBuilder
           .Entity<User>()
           .ToTable("Users");
        }
    }
}

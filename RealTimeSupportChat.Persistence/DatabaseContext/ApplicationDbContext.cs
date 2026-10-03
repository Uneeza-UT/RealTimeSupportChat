using Microsoft.EntityFrameworkCore;
using RealTimeSupportChat.Domain;
using RealTimeSupportChat.Domain.Common;

namespace RealTimeSupportChat.Persistence.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        DbSet<Ticket> Tickets { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Attachment> Attachments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in base.ChangeTracker.Entries<BaseEntity>()
                .Where(q => q.State == EntityState.Added)) 
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeSupportChat.Domain;
using RealTimeSupportChat.Domain.Enums;

namespace RealTimeSupportChat.Persistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            // Store the enum values for ticket status as strings
            // instead of numeric values for database readability.

            builder.Property(q => q.Status)
                .HasConversion<string>()
                .HasDefaultValue(TicketStatus.Open);


            builder.Property(q => q.Subject)
                .HasMaxLength(200);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeSupportChat.Domain;

namespace RealTimeSupportChat.Persistence.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.Property(q => q.FileName)
                .HasMaxLength(200);


            builder.Property(q => q.FilePath)
                .HasMaxLength(500);


            builder.Property(q => q.ContentType)
                .HasMaxLength(100);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RealTimeSupportChat.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "7f3a9c21-5d84-4e67-a2b9-1c6f8d3e4a52",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                },
                
                new IdentityRole
                {
                    Id = "c4a17e63-82fb-4d95-b038-7e2c9a5164fd",
                    Name = "SupportManager",
                    NormalizedName = "SUPPORTMANAGER"
                },

                new IdentityRole
                {
                    Id = "b8e2d714-39ac-4f56-91de-6a7c2b5f8031",
                    Name = "SupportAgent",
                    NormalizedName = "SUPPORTAGENT"
                }
            );
        }
    }
}

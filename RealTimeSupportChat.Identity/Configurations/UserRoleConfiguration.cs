using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RealTimeSupportChat.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "d91f4a72-6c38-4b15-9e27-53a8c1f604bd",
                    RoleId = "7f3a9c21-5d84-4e67-a2b9-1c6f8d3e4a52" // Customer
                },

                new IdentityUserRole<string>
                {
                    UserId = "a63e8b19-2d74-4f06-bc51-97d4e2a83f60",
                    RoleId = "c4a17e63-82fb-4d95-b038-7e2c9a5164fd" // SupportManager
                },

                new IdentityUserRole<string>
                {
                    UserId = "f27c5d04-91ae-438b-86f2-7c3e1a95b640",
                    RoleId = "b8e2d714-39ac-4f56-91de-6a7c2b5f8031" // SupportAgent
                }
            );
        }
    }
}

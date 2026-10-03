using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeSupportChat.Identity.Models;

namespace RealTimeSupportChat.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(
                new ApplicationUser
                {
                    Id = "d91f4a72-6c38-4b15-9e27-53a8c1f604bd",
                    Email = "customer@example.com",
                    NormalizedEmail = "CUSTOMER@EXAMPLE.COM",
                    FirstName = "Test",
                    LastName = "Customer",
                    UserName = "customer@example.com",
                    NormalizedUserName = "CUSTOMER@EXAMPLE.COM",
                    PasswordHash = passwordHasher.HashPassword(null, "Customer1234!"),
                    EmailConfirmed = true
                },

                new ApplicationUser
                {
                    Id = "a63e8b19-2d74-4f06-bc51-97d4e2a83f60",
                    Email = "support.manager@example.com",
                    NormalizedEmail = "SUPPORT.MANAGER@EXAMPLE.COM",
                    FirstName = "Support",
                    LastName = "Manager",
                    UserName = "support.manager@example.com",
                    NormalizedUserName = "SUPPORT.MANAGER@EXAMPLE.COM",
                    PasswordHash = passwordHasher.HashPassword(null, "Manager1234!"),
                    EmailConfirmed = true
                },

                new ApplicationUser
                {
                    Id = "f27c5d04-91ae-438b-86f2-7c3e1a95b640",
                    Email = "support.agent@example.com",
                    NormalizedEmail = "SUPPORT.AGENT@EXAMPLE.COM",
                    FirstName = "Support",
                    LastName = "Agent",
                    UserName = "support.agent@example.com",
                    NormalizedUserName = "SUPPORT.AGENT@EXAMPLE.COM",
                    PasswordHash = passwordHasher.HashPassword(null, "Agent1234!"),
                    EmailConfirmed = true
                }
            );
        }
    }
}

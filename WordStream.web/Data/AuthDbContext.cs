using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WordStream.web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Role (Users, Admin, SuperAdmin)

            var adminRoleId = "8424e9bd-0b3f-4234-910c-964bd75b07e7";
            var superAdminRoleId = "45a856c3-4e35-4898-b31b-d68b79c54ed9";
            var userRoleId = "283c57af-a193-4e7b-bfda-8b5099ac729f";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },

                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },

                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }

            };

            builder.Entity<IdentityRole>().HasData(roles);

            // Seed SuperAdmin
            var superAdminId = "68acc9ff-f436-4d90-a171-40a81a4a30d8";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@wordstream.com",
                Email = "superadmin@wordstream.com",
                NormalizedEmail = "superadmin@wordstream.com".ToUpper(),
                NormalizedUserName = "superadmin@wordstream.com".ToUpper(),
                Id = superAdminId
            };

            // Here creatd a password for SuperAdmin
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "superadmin@123");


            //pass to builder
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add All the Roles to SuperAdminUser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },

                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },

                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
            


        }
    }
}

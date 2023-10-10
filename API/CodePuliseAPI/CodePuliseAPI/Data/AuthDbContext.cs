using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePuliseAPI.Data
{
    public class AuthDbContext:IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "8c815609-d110-4a82-88ef-498e2af9b65a";
            var writerRoleId = "5b00e15f-38b2-428c-ba50-f1ba8ba51444";
            //create reader and write Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id=readerRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper(),
                    ConcurrencyStamp=readerRoleId
                },new IdentityRole()
                {
                    Id=writerRoleId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper(),
                    ConcurrencyStamp=writerRoleId
                },

            };
            //send the roles
            builder.Entity<IdentityRole>().HasData(roles);

            var adminUserId = "563be3d7-1928-45b3-a707-8403dd8033d2";
            //create an admin user
            var admin = new IdentityUser()
            {
                Id= adminUserId,
                UserName="admin@gmail.com",
                Email= "admin@gmail.com",
                NormalizedEmail= "admin@gmail.com".ToUpper(),
                NormalizedUserName= "admin@gmail.com".ToUpper()
            };
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            builder.Entity<IdentityUser>().HasData(admin);
            //give roles to admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId=adminUserId,
                    RoleId=readerRoleId
                },
                new()
                {
                    UserId=adminUserId,
                    RoleId=writerRoleId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }



    }
}

using Microsoft.AspNetCore.Identity;
using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.DAL
{
    public class SeedData
    {
        public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin"
                });
            }
            if (!await roleManager.RoleExistsAsync("Moderator"))
            {
                await roleManager.CreateAsync(new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = "Moderator"
                });
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = "User"
                });
            }
        }

        public static async Task SeedAdminAsync(UserManager<User> userManager)
        {
            //Seed Default User(Admin)
            var defaultAdmin1 = new User
            {
                Id = Guid.NewGuid(),
                UserName = "admin2137",
                Email = "admin@gmail.com",
                PhoneNumber = "123456789",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            if (userManager.Users.All(u => u.Id != defaultAdmin1.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultAdmin1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultAdmin1, "Haslo1");
                    await userManager.AddToRoleAsync(defaultAdmin1, "Admin");
                }
            }
        }
    }
}

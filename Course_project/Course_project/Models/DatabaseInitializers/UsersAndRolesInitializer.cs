using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Course_project.Models.DatabaseInitializers
{
    /// <summary>
    /// Initializer class for users and roles in database
    /// </summary>
    public class UsersAndRolesInitializer
    {
        /// <summary>
        /// Initialize users and roles in database
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <param name="roleManager">Role manager</param>
        /// <returns></returns>
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string adminLogin = "admin";
            string password = "Admin-entry12";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminLogin) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminLogin, Nickname = adminLogin };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}

using System;
using Microsoft.AspNetCore.Identity;

namespace Web_HW03
{
    internal class MyIdentityData
    {
        public const string AdminRoleName = "Admin";
        public const string EditorRoleName = "Editor";
        public const string ContributorRoleName = "Contributor";

        internal static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in new[] { AdminRoleName, EditorRoleName, ContributorRoleName })
            {
                var role = roleManager.FindByNameAsync(roleName).Result;
                if (role == null)
                {
                    role = new IdentityRole(roleName);
                    var result = roleManager.CreateAsync(role).Result;
                }
            }

            foreach (var userName in new[] { "admin@snow.edu", "editor@snow.edu", "contributor@snow.edu" })
            {
                var user = userManager.FindByNameAsync(userName).Result;
                if (user == null)
                {
                    user = new IdentityUser(userName);
                    user.Email = userName;
                    var result = userManager.CreateAsync(user, "P@ssword1").Result;
                }
                if (userName.StartsWith("admin"))
                {
                    var result2 = userManager.AddToRoleAsync(user, AdminRoleName).Result;
                }
                if (userName.StartsWith("editor"))
                {
                    var result3 = userManager.AddToRoleAsync(user, EditorRoleName).Result;
                }
                if (userName.StartsWith("contributor"))
                {
                    var result4 = userManager.AddToRoleAsync(user, ContributorRoleName).Result;
                }
            }
        }
    }
}
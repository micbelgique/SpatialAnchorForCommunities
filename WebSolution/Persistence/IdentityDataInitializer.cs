using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using System.Security.Claims;

namespace Persistence
{
    public static class IdentityDataInitializer
    {
        public static void SeedData(UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            // used to initialize first user in database
            /*
            var username = $"";
            if (userManager.FindByNameAsync(username).Result == null)
            {
                var user = new User { UserName = username, Email = "" };
                var createUserResult = userManager.CreateAsync(user, "").Result;
                if (createUserResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Super_Admin").Wait();
                }
            }
            */
        }

        public static void SeedRoles(RoleManager<Role> roleManager)
        {
            var roles = new List<string>()
            {
                "Community_Admin",
                "Super_Admin",
                "User"
            };

            foreach (var roleTitle in roles)
            {
                if (!roleManager.RoleExistsAsync(roleTitle).Result)
                {
                    var role = new Role { Name = roleTitle };
                    var createRoleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }
    }
}

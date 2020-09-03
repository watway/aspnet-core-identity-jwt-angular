using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Build the application host
            var host = CreateHostBuilder(args).Build();

            // Create a scope, seed the database
            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                // TODO: Add seed

                // Seed the Roles
                var roles = (Role[])Enum.GetValues(typeof(Role));
                foreach(var r in roles)
                {
                    // create an identity role object out of the enum value
                    var identityRole = new IdentityRole
                    {
                        Id = r.GetRoleName(),
                        Name = r.GetRoleName()
                    };

                    // Create the role if it doesn't already exist
                    if (!await roleManager.RoleExistsAsync(roleName: identityRole.Name))
                    {
                        var result = await roleManager.CreateAsync(identityRole);
                        if (!result.Succeeded)
                        {
                            // FIXME: don't throw exception
                            throw new Exception("Create role failed");
                        }
                    }
                }
            }

            // Run the application
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

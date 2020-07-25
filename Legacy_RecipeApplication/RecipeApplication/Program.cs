using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RecipeApplication.Data;
using Serilog;

namespace RecipeApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host =BuildWebHost(args);
            
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var ctx = serviceProvider.GetRequiredService<AppDbContext>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var task = userManager.CreateAsync(new ApplicationUser { Email="test@test.com", UserName = "MaxMuster" }, "Test123!").GetAwaiter().GetResult();
                if (!task.Succeeded)
                    return;
                var user = userManager.FindByEmailAsync("test@test.com").GetAwaiter().GetResult();
                
                DataGenerator.Initialize(serviceProvider, user.Id);
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}

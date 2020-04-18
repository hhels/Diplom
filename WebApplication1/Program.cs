using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

using System;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace WebApplication1
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateWebHostBuilder(args).Build().Run();
        //}

            //создание при запуске ролей пользователей из RoleInitializer
        public static async Task Main(string[] args)
        {
            //CreateHostBuilder(args).Build();
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<SiteUser>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await RoleInitializer.InitializeAsync(userManager, rolesManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .ConfigureLogging(config => { config.ClearProviders(); })
                          .UseSerilog((hostingContext, loggerConfiguration) =>
                          {
                              loggerConfiguration
                                      .ReadFrom.Configuration(hostingContext.Configuration);
                          })
                          .UseStartup<Startup>();
        }
    }
}
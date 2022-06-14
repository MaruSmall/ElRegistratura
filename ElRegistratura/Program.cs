using ElRegistratura.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace ElRegistratura
{
    public class Program
    {

        
        public static async Task Main(string[] args )
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var context = services.GetRequiredService<ApplicationDbContext>();
                try
                {
                   
                    if (context.Database.CanConnect() == true)
                    {
                        Console.WriteLine("Connection established!");
                        var userManager = services.GetRequiredService<UserManager<Models.User>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        await SeedData.SeedRolesAsync(userManager, roleManager);
                        await SeedData.SeedSuperAdminAsync(userManager, roleManager);

                        host.Run();
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Connection failed!!!");
                        ErrorDB errorDB = new ErrorDB();
                        errorDB.ErrorDBView();
                        
                    }

                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "Произошла ошибка заполнения Базы данных.");
                }
            }
            
         
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

      

    }
}

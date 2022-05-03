using ElRegistratura.Data;
using ElRegistratura.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElRegistratura
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //     bool isAvalaible = await db.Database.CanConnectAsync();

            //    if (isAvalaible)
            //    {
            //        Console.WriteLine("База данных доступна");
                    var host = CreateHostBuilder(args).Build();
                    using (var scope = host.Services.CreateScope())
                    {
                        var services = scope.ServiceProvider;
                        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                        try
                        {
                            var context = services.GetRequiredService<ApplicationDbContext>();
                            if (context.Database.CanConnect() == true)
                            {
                                 Console.WriteLine("Connection established!");
                                 var userManager = services.GetRequiredService<UserManager<User>>();
                                 var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                                 await SeedData.SeedRolesAsync(userManager, roleManager);
                                 await SeedData.SeedSuperAdminAsync(userManager, roleManager);
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
                    host.Run();
            //    }

            //    else
            //    {
            //        Console.WriteLine("База данных не доступна");
            //    }

            //}
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

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
using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using ElRegistratura.Controllers;

namespace ElRegistratura
{
    public class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("5351994087:AAE7yYgY08R7p_qWrO-6v0ki8OVxlxZkJGM");
      
        public static async Task Main(string[] args)
        {
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
                        var userManager = services.GetRequiredService<UserManager<Models.User>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        await SeedData.SeedRolesAsync(userManager, roleManager);
                        await SeedData.SeedSuperAdminAsync(userManager, roleManager);
                        
                        //bot
                        Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

                        var cts = new CancellationTokenSource();
                        var cancellationToken = cts.Token;
                        var receiverOptions = new ReceiverOptions
                        {
                            AllowedUpdates = { }, // receive all update types
                        };
                        bot.StartReceiving(
                            HandleUpdateAsync,
                            HandleErrorAsync,
                            receiverOptions,
                            cancellationToken
                        );
                        //TelegramBotController tb = new TelegramBotController();
                        //tb.

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

        
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Models.User user = new Models.User();
            //var s = context.Users.ToList();
          //  var s = context.Users;
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать, добрый путник!");
                    return;
                }
                if(message.Text.ToLower() == "/phone")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Введите ваш номер телефона для поиска вашей записи на прием");
                   
                    return;
                }
               // 

                await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
            }
            
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}

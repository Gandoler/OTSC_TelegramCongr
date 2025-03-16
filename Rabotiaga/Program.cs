using Domain.Interfaces;
using Domain.Interfaces.IDBProxy;
using Domain.Interfaces.IServices;
using Domain.Interfaces.ITgSubProxy;
using Infrastructure.Data;
using Infrastructure.Data.DBProxy;
using Interfaces.Business;
using Interfaces.Business.Service;
using Interfaces.Business.TgSubProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rabotiaga;
using Serilog;
using Telegram.Bot;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        // Читаем настройки
        var apiKey = configuration["TelegramBot:ApiKey"]
                     ?? throw new Exception("ApiKey is missing");
        var dbProxy = configuration["ApiSettings:DbProxy"]
                      ?? throw new Exception("DbProxy is missing");
        var Tgsub = configuration["ApiSettings:TGSUBS"]
                     ?? throw new Exception("ApiUrl is missing");

        // Регистрируем зависимости
        services.AddSingleton(new TelegramBotClient(apiKey));
        services.AddSingleton<IBotCommandHandler, BotCommandHandler>();
        services.AddSingleton<ITgSubProxy, TgSubProxy>(); 
        services.AddSingleton<ICongratulationService, CongratulationService>();

        // Добавляем недостающие зависимости
        services.AddSingleton<IGetCongr, GetCongr>(); 
        services.AddSingleton<IGetPozdrikId, GetPozdrikId>(); 
        services.AddSingleton<IGetTgId, GetTgId>(); 
        services.AddSingleton<IGetTodayBDayFriends, GetTodayBDayFriends>(); 
        services.AddSingleton<ITgBotRepository, TgBotRepository>();
        

        services.AddSingleton<ITgBotRepository, TgBotRepository>();
        
        services.AddHttpClient<ITgSubProxy, TgSubProxy>(client =>
        {
            client.BaseAddress = new Uri(Tgsub);
        });

        services.AddSingleton<ITgBotRepository, TgBotRepository>();


        services.AddLogging();
        services.AddSingleton<TelegramBotService>();
        services.AddHostedService<Worker>();
    })
    .UseSerilog((context, config) => config.WriteTo.Console().WriteTo.File("logs/logs.txt"))
    .Build();

await builder.RunAsync();

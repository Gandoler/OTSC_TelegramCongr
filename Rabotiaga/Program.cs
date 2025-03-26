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
using Microsoft.Extensions.Logging;
using Rabotiaga;
using Serilog;
using Telegram.Bot;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        // Используем настройки из окружения для подключения
        var env = hostContext.HostingEnvironment;
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
        else
        {
            // В режиме продакшн или публикации - настройки загружаем из переменных окружения
            config.AddEnvironmentVariables();
        }
    })
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        // Чтение настроек из окружения или конфигурационного файла
        var apiKey = configuration["TelegramBot:ApiKey"]
                     ?? throw new Exception("ApiKey is missing");
        var dbProxy = configuration["ApiSettings:DbProxy"]
                      ?? throw new Exception("DbProxy is missing");
        var tgSub = configuration["ApiSettings:TGSUBS"]
                     ?? throw new Exception("TGSUBS is missing");

        // Регистрируем зависимости
        services.AddSingleton(new TelegramBotClient(apiKey));
        services.AddSingleton<IBotCommandHandler, BotCommandHandler>();
        services.AddSingleton<ITgSubProxy, TgSubProxy>(); 
        services.AddSingleton<ICongratulationService, CongratulationService>();

        services.AddSingleton<IGetCongr, GetCongr>(); 
        services.AddSingleton<IGetPozdrikId, GetPozdrikId>(); 
        services.AddSingleton<IGetTgId, GetTgId>(); 
        services.AddSingleton<IGetTodayBDayFriends, GetTodayBDayFriends>(); 
        services.AddSingleton<ITgBotRepository, TgBotRepository>();
        services.AddTransient<IGetTodayBDayFriends, GetTodayBDayFriends>();

        services.AddHttpClient("DbProxyClient", client =>
        {
            var dbProxyUrl = configuration["ApiSettings:DbProxy"] ?? throw new Exception("DbProxy is missing in configuration");
            client.BaseAddress = new Uri(dbProxyUrl);
        });

        services.AddHttpClient("TgSubClient", client =>
        {
            var tgSubUrl = configuration["ApiSettings:TGSUBS"] ?? throw new Exception("TGSUBS is missing in configuration");
            client.BaseAddress = new Uri(tgSubUrl);
        });

        // Добавляем недостающие зависимости
        services.Configure<HostOptions>(options =>
        {
            options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        services.AddLogging();
        services.AddSingleton<TelegramBotService>();
        services.AddHostedService<Worker>();
    })
    .UseSerilog((context, config) => config.WriteTo.Console().WriteTo.File("logs/logs.txt"))
    .Build();

await builder.RunAsync();

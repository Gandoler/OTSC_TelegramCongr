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

// Инициализация Serilog до создания хоста
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureAppConfiguration((hostContext, config) =>
        {
            var env = hostContext.HostingEnvironment;
            if (env.IsDevelopment())
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            }
            else
            {
                config.Sources.Clear(); 
                config.AddEnvironmentVariables();
            }
        })
        .ConfigureServices((hostContext, services) =>
        {
            var configuration = hostContext.Configuration;

            var apiKey = configuration["TelegramBot:ApiKey"]
                         ?? throw new Exception("ApiKey is missing");
            var dbProxy = configuration["ApiSettings:DbProxy"]
                          ?? throw new Exception("DbProxy is missing");
            var tgSub = configuration["ApiSettings:TGSUBS"]
                         ?? throw new Exception("TGSUBS is missing");


            services.AddSingleton(Log.Logger);
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
                client.BaseAddress = new Uri(dbProxy);
            });

            services.AddHttpClient("TgSubClient", client =>
            {
                client.BaseAddress = new Uri(tgSub);
            });

            services.Configure<HostOptions>(options =>
            {
                options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            });

            services.AddLogging();
            
            Log.Information("apiKey = {ApiKey}", apiKey);
            Log.Information("dbProxy = {DbProxy}", dbProxy);
            Log.Information("tgSub = {TgSub}", tgSub);
            services.AddSingleton<TelegramBotService>();
            services.AddHostedService<Worker>();
        })
        .Build();

    await builder.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Приложение завершилось с критической ошибкой");
}
finally
{
    Log.CloseAndFlush();
}
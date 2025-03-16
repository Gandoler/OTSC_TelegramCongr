using Interfaces.Business.Service;

namespace Rabotiaga;

public class Worker : BackgroundService
{

    private readonly TelegramBotService _botService;
    private readonly TimeSpan _runTime = TimeSpan.FromMinutes(52);
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger, TelegramBotService botService)
    {
        _logger = logger;
        _botService = botService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Сервис запущен...");

        // Запуск бота
        _ = _botService.StartAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            DateTime now = DateTime.Now;
            DateTime nextRun = now.Date.AddHours(now.Hour).Add(_runTime);

            if (now > nextRun)
                nextRun = now.Add(_runTime);

            TimeSpan delay = nextRun - now;
            _logger.LogInformation("Следующий запуск поздравлений в: {Time}", nextRun);

            try
            {
                await Task.Delay(delay, stoppingToken);
                await _botService.SendCongratulate(stoppingToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Сервис остановлен.");
            }
        }
    }
}
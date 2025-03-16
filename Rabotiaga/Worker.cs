using Interfaces.Business.Service;

namespace Rabotiaga;

public class Worker : BackgroundService
{
    private readonly ILogger _logger;
    private readonly TelegramBotService _botService;
    private readonly TimeSpan _runTime = TimeSpan.FromHours(10);

    public Worker(ILogger logger ,TelegramBotService botService)
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
            DateTime nextRun = now.Date + _runTime;
            if (now > nextRun)
                nextRun = nextRun.AddDays(1);

            TimeSpan delay = nextRun - now;
            _logger.LogInformation("Следующий запуск поздравлений в: {Time}", nextRun);

            try
            {
                await Task.Delay(delay, stoppingToken);
                await _botService.SendCongratulate(null, 0, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Сервис остановлен.");
            }
        }
    }
}
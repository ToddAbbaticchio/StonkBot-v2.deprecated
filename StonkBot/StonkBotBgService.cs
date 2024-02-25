using StonkBot.StonkBot._SBResources;
using StonkBot.StonkBot.Actions;

namespace StonkBot.StonkBot
{
    public sealed class WindowsBackgroundService : BackgroundService
    {
        private readonly IStonkBotScheduler _sbScheduler;
        private readonly ILogger<WindowsBackgroundService> _logger;

        public WindowsBackgroundService(IStonkBotScheduler sbScheduler, ILogger<WindowsBackgroundService> logger)
        {
            _sbScheduler = sbScheduler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cToken)
        {
            try
            {
                var actionSchedule = new ActionSchedule();

                while (!cToken.IsCancellationRequested)
                {
                    actionSchedule = await _sbScheduler.ActionHandler(actionSchedule, cToken);
                    await Task.Delay(TimeSpan.FromSeconds(30), cToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
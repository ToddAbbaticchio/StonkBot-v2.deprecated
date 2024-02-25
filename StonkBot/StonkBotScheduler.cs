using StonkBot.StonkBot._SBResources;
using StonkBot.StonkBot.Actions;
using StonkBot.StonkBot.Utilities;

namespace StonkBot.StonkBot
{
    public interface IStonkBotScheduler
    {
        Task<ActionSchedule> ActionHandler(ActionSchedule actionSchedule, CancellationToken cToken);
    }

    internal class StonkBotScheduler : IStonkBotScheduler
    {
        private readonly IStonkBotActions _sbActions;
        private readonly ISpecialActions _specialActions;
        private readonly ILogger<StonkBotScheduler> _logger;
        private readonly ConsoleWriter _cWriter;

        
        public StonkBotScheduler(IStonkBotActions sbActions, ISpecialActions specialActions, ILogger<StonkBotScheduler> logger, ConsoleWriter cWriter)
        {
            _sbActions = sbActions;
            _specialActions = specialActions;
            _logger = logger;
            _cWriter = cWriter;
        }

        public async Task<ActionSchedule> ActionHandler(ActionSchedule actionSchedule, CancellationToken cToken)
        {
            //await _specialActions.DumpTable(cToken);
            
            // Isoloation Test Actions
            //await _specialActions.IsolateScanForAlerts("CSIQ", Alerts.FourHandAlert, new DateTime(2023, 1, 30).Date, cToken);

            // Manual calls of SBActions
            //await _specialActions.ScanFixMissingMonths(1, cToken);
            //await _sbActions.UpdateAllCalculatedFields(false, cToken);

            //Console.WriteLine("breakpoint here");

            // action dictionary (bus driver)
            var actionHandler = new Dictionary<string, Func<Task>>
            {
                { "IpoScrape", () => _sbActions.IpoScrape(cToken) },
                { "IpoCheck", () => _sbActions.IpoCheck(cToken) },
                { "UpdateIndustryAnalysisStocks", () => _sbActions.UpdateIndustryAnalysisStocks(cToken) },
                { "UpdateAllCalculatedFields", () => _sbActions.UpdateAllCalculatedFields(false, cToken) },
                { "SendDiscordAlerts", () => _sbActions.SendDiscordAlerts(cToken) },
            };

            // If actionSchedule isn't from today, make a new one
            if (DateTime.Today != actionSchedule.CreatedOn)
            {
                actionSchedule = new ActionSchedule();
                _cWriter.WriteMessage($"Generated new ActionSchedule!", ConsoleColor.DarkCyan);
            }

            // Get current time in unix format for startTime / endTime evaluation
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();

            // Check the action schedule.  Do the things.
            foreach (var action in actionSchedule.ActionInfos.Where(action => now >= action.StartTime && now <= action.EndTime))
            {
                // Set next run time
                while (action.StartTime <= now)
                {
                    action.StartTime += action.Interval;
                }

                // Run the action
                try
                {
                    await actionHandler[action.Method]();
                    _cWriter.WriteMessage($"Ran {action.Method} successfully! Next run queued for: {TimeHelper.GetReadableTime(action.StartTime)}", ConsoleColor.DarkCyan);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error running {method}: {errMsg}", action.Method, ex.Message);
                }
            }
            return actionSchedule;
        }
    }
}
namespace StonkBot.StonkBot._SBResources
{
    public class ActionSchedule
    {
        public DateTime CreatedOn { get; set; } = DateTime.Today;
        public List<ActionInfo> ActionInfos { get; set; } = new List<ActionInfo>();
        
        public ActionSchedule()
        {
            ActionInfos.Add(new ActionInfo
            {
                Method = "IpoScrape",
                StartTime = TimeHelper.GetTodayTimeAsLong("00:00:00"),
                EndTime = TimeHelper.GetTodayTimeAsLong("23:59:59"),
                Interval = (long)Intervals.Hour3
            });

            ActionInfos.Add(new ActionInfo
            {
                Method = "IpoCheck",
                StartTime = TimeHelper.GetTodayTimeAsLong("8:01:00 PM"),
                EndTime = TimeHelper.GetTodayTimeAsLong("23:59:59"),
                Interval = (long)Intervals.Hour1
            });

            ActionInfos.Add(new ActionInfo
            {
                Method = "UpdateIndustryAnalysisStocks",
                StartTime = TimeHelper.GetTodayTimeAsLong("8:01:00 PM"),
                EndTime = TimeHelper.GetTodayTimeAsLong("23:59:59"),
                Interval = (long)Intervals.Hour1
            });

            ActionInfos.Add(new ActionInfo
            {
                Method = "UpdateAllCalculatedFields",
                StartTime = TimeHelper.GetTodayTimeAsLong("8:30:00 PM"),
                EndTime = TimeHelper.GetTodayTimeAsLong("23:59:59"),
                Interval = (long)Intervals.Hour1
            });

            ActionInfos.Add(new ActionInfo
            {
                Method = "SendDiscordAlerts",
                StartTime = TimeHelper.GetTodayTimeAsLong("8:30:00 PM"),
                EndTime = TimeHelper.GetTodayTimeAsLong("23:59:59"),
                Interval = (long)Intervals.Daily
            });
        }
    }

    public class ActionInfo
    {
        public string Method { get; set; } = null!;
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public long Interval { get; set; }
    }
}
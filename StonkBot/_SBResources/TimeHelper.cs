namespace StonkBot.StonkBot._SBResources
{
    public class TimeHelper
    {
        public static long GetTodayTimeAsLong(string time)
        {
            var now = DateTime.Now;
            return DateTimeOffset.Parse($"{now.Month}/{now.Day}/{now.Year} {time}").ToUnixTimeSeconds();
        }

        public static string GetReadableTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime.ToString("hh:mm tt");
        }

        // USE THIS FOR HISTORY DATA
        public static DateTime ConvertLongToUTCDateTime(long tdTime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(tdTime).UtcDateTime.Date;
        }

        public static List<DateTime> GetPreviousTradeDays(DateTime startDate, int count)
        {
            var holidays = MarketHolidays.GetDates();
            
            var prevDaysList = new List<DateTime>();
            var checkDate = startDate;
            
            while (prevDaysList.Count < count)
            {
                while (checkDate == startDate || checkDate.DayOfWeek == DayOfWeek.Sunday || checkDate.DayOfWeek == DayOfWeek.Saturday || holidays.Contains(checkDate))
                {
                    checkDate = checkDate.AddDays(-1);
                }

                prevDaysList.Add(checkDate);
                startDate = checkDate;
            }
            
            return prevDaysList;
        }
    }
}
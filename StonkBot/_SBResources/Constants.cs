namespace StonkBot.StonkBot._SBResources
{
    public class Constants
    {
        public const string webullScrapeUrl = "https://www.webull.com/quote/us/ipo?hl=en";
        public const string tdRedirectUrl = "https://localhost:8080/";
        public const string tdTokenUrl = "https://api.tdameritrade.com/v1/oauth2/token";
        public const string tdQuoteUrl = "https://api.tdameritrade.com/v1/marketdata/quotes?symbol=";


        public const string tdAmeritradeBaseUrl = "https://api.tdameritrade.com/v1";
        public const string tdAmeritradeTokenPath = "oauth2/token";

        public const string tdAmeritradeClientId = "redacted@AMER.OAUTHAP";
    }

    public class MarketHolidays
    {
        static public List<DateTime> GetDates()
        {
            var holidayList = new List<DateTime>();
            holidayList.Add(new DateTime(2022, 1, 17, 0, 0, 0));
            holidayList.Add(new DateTime(2022, 2, 21, 0, 0, 0));
            holidayList.Add(new DateTime(2022, 4, 15, 0, 0, 0));
            holidayList.Add(new DateTime(2022, 5, 30, 0, 0, 0));
            holidayList.Add(new DateTime(2022, 6, 20, 0, 0, 0));
            holidayList.Add(new DateTime(2022, 7, 4, 0, 0, 0));
            holidayList.Add(new DateTime(2022, 9, 5, 0, 0, 0));
            holidayList.Add(new DateTime(2022, 11, 24, 0, 0, 0));
            holidayList.Add(new DateTime(2022, 12, 26, 0, 0, 0));

            holidayList.Add(new DateTime(2023, 1, 2, 0, 0, 0));
            holidayList.Add(new DateTime(2023, 1, 16, 0, 0, 0));
            holidayList.Add(new DateTime(2023, 2, 20, 0, 0, 0));
            holidayList.Add(new DateTime(2023, 4, 7, 0, 0, 0));
            holidayList.Add(new DateTime(2023, 5, 29, 0, 0, 0));
            holidayList.Add(new DateTime(2023, 6, 19, 0, 0, 0));
            holidayList.Add(new DateTime(2023, 7, 4, 0, 0, 0));
            holidayList.Add(new DateTime(2023, 9, 4, 0, 0, 0));
            holidayList.Add(new DateTime(2023, 11, 23, 0, 0, 0));
            holidayList.Add(new DateTime(2023, 12, 25, 0, 0, 0));

            holidayList.Add(new DateTime(2024, 1, 1, 0, 0, 0));
            holidayList.Add(new DateTime(2024, 1, 15, 0, 0, 0));
            holidayList.Add(new DateTime(2024, 2, 19, 0, 0, 0));
            holidayList.Add(new DateTime(2024, 3, 29, 0, 0, 0));
            holidayList.Add(new DateTime(2024, 5, 27, 0, 0, 0));
            holidayList.Add(new DateTime(2024, 6, 19, 0, 0, 0));
            holidayList.Add(new DateTime(2024, 7, 4, 0, 0, 0));
            holidayList.Add(new DateTime(2024, 9, 2, 0, 0, 0));
            holidayList.Add(new DateTime(2024, 11, 28, 0, 0, 0));
            holidayList.Add(new DateTime(2024, 12, 25, 0, 0, 0));

            holidayList.Add(new DateTime(2025, 1, 1, 0, 0, 0));
            holidayList.Add(new DateTime(2025, 1, 20, 0, 0, 0));
            holidayList.Add(new DateTime(2025, 2, 17, 0, 0, 0));
            holidayList.Add(new DateTime(2025, 4, 18, 0, 0, 0));
            holidayList.Add(new DateTime(2025, 5, 26, 0, 0, 0));
            holidayList.Add(new DateTime(2025, 6, 19, 0, 0, 0));
            holidayList.Add(new DateTime(2025, 7, 4, 0, 0, 0));
            holidayList.Add(new DateTime(2025, 9, 1, 0, 0, 0));
            holidayList.Add(new DateTime(2025, 11, 27, 0, 0, 0));
            holidayList.Add(new DateTime(2025, 12, 25, 0, 0, 0));

            holidayList.Add(new DateTime(2026, 1, 1, 0, 0, 0));
            holidayList.Add(new DateTime(2026, 1, 19, 0, 0, 0));
            holidayList.Add(new DateTime(2026, 2, 16, 0, 0, 0));
            holidayList.Add(new DateTime(2026, 4, 3, 0, 0, 0));
            holidayList.Add(new DateTime(2026, 5, 25, 0, 0, 0));
            holidayList.Add(new DateTime(2026, 6, 19, 0, 0, 0));
            holidayList.Add(new DateTime(2026, 7, 3, 0, 0, 0));
            holidayList.Add(new DateTime(2026, 9, 7, 0, 0, 0));
            holidayList.Add(new DateTime(2026, 11, 26, 0, 0, 0));
            holidayList.Add(new DateTime(2026, 12, 25, 0, 0, 0));
            
            return holidayList;
        }
    }

    public class DiscordConstants
    {
        public const string VolumeAlertWebHook = "redacted";
        public const string UpperShadowWebHook = "redacted";
        public const string FourHandWebHook = "redacted";
        public const string IpoWatchWebHook = "redacted";

        public const ulong VolumeAlertMessageId = redacted;
        public const ulong VolumeAlert2MessageId = redacted;
        public const ulong UpperShadowMessageId = redacted;
        public const ulong FourHandMessageId = redacted;
    }
}
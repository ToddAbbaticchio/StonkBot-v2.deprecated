namespace StonkBot.StonkBot._SBResources
{
    public interface IUrlBuilder
    {
        public string GetQuoteUrl(string symbol);
        public string GetQuotesUrl(List<string> symbol);
        public string GetHistoricalQuoteUrl(string symbol, string periodType, string period, string frequencyType);
    }

    internal class UrlBuilder : IUrlBuilder
    {
        public string GetQuoteUrl(string symbol)
        {
            //return $"https://api.tdameritrade.com/v1/marketdata/quotes?symbol={symbol}";
            return $"https://api.tdameritrade.com/v1/marketdata/{symbol}/quotes";
        }

        public string GetQuotesUrl(List<string> symbols)
        {
            return $"https://api.tdameritrade.com/v1/marketdata/quotes?symbol={string.Join("%2C", symbols)}";
        }

        public string GetHistoricalQuoteUrl(string symbol, string periodType, string period, string frequencyType)
        {
            return $"https://api.tdameritrade.com/v1/marketdata/{symbol}/pricehistory?periodType={periodType}&period={period}&frequencyType={frequencyType}&frequency=1";
        }
    }
}
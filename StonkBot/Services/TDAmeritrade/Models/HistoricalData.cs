using StonkBot.StonkBot._SBResources;

namespace StonkBot.StonkBot.Services.TDAmeritrade.Models
{
    public class HistoricalData
    {
        public List<Candle> candles { get; set; } = new();
        public string symbol { get; set; } = null!;
        public bool empty { get; set; }
    }

    public class Candle
    {
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public decimal volume { get; set; }
        public long datetime { get; set; }
        public DateTime? goodDateTime { get; set; }
    }
}
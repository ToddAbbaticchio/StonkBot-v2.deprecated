namespace StonkBot.StonkBot.Database.Entities.IndustryAnalysis
{
    public class IAInfo
    {
        public string GICSSector { get; set; } = null!;
        public string Industry { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal MarketCap { get; set; }
        public decimal AvgVolume { get; set; }
        public bool InSpy { get; set; }
        public bool InQQQ { get; set; }
        public bool InIWM { get; set; }
        public bool InDIA { get; set; }
    }
}
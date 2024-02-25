namespace StonkBot.StonkBot.Database.Entities.IndustryAnalysis;
public class IAHistoricalData
{
    public string Symbol { get; set; } = null!;
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }
    public DateTime Datetime { get; set; }
    public string? Sector { get; set; }
    public string? Industry { get; set; }
    public string? Category { get; set; }
    public string? FromYesterday { get; set; }
    public bool? UpToday { get; set; }
    public string? VolumeAlert { get; set; }
    public string? VolumeAlert2 { get; set; }
    public string? FiveDayStable { get; set; }
    public bool? UpperShadow { get; set; }
    public string? AboveUpperShadow { get; set; }
    public string? FHTargetDay { get; set; }
    public string? LastFHTarget { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace StonkBot.StonkBot.Services.WebScrape.Models
{
    public class IpoListing
    {
        [Key]public string Symbol { get; set; } = null!;
        public string? Name { get; set; }
        public string OfferingPrice { get; set; } = null!;
        public string? OfferAmmount { get; set; }
        public string? OfferingEndDate { get; set; }
        public string ExpectedListingDate { get; set; } = null!;
        public string ScrapeDate { get; set; } = null!;
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? Low { get; set; }
        public decimal? High { get; set; }
        public decimal? Volume { get; set; }
        public ulong? DiscordMessageId { get; set; }
        public DateTime? DiscordMessageDate { get; set; }

        public bool SurpassedOpeningDay()
        {
            return DiscordMessageId != null;
        }
        public bool OpeningDayInfoRecorded()
        {
            return Open != null && Close != null && Low != null && High != null && Volume != null;
        }
    }
}
namespace StonkBot.StonkBot.Actions.Models
{
    public class MissingInfo
    {
        public string Symbol { get; set; } = null!;
        public DateTime Date { get; set; } = new DateTime()!;
    }
}
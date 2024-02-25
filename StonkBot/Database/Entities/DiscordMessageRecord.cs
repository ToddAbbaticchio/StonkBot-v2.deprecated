using StonkBot.StonkBot.Services.DiscordMessageService.Enums;

namespace StonkBot.StonkBot.Database.Entities;
public class DiscordMessageRecord
{
    public long Id { get; set; }
    public Channels Channel { get; set; }
    public DateTime SentOn { get; set; } = DateTime.Today.Date;
    public string? Message { get; set; }
}
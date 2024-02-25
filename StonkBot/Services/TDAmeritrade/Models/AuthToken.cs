using System.ComponentModel.DataAnnotations;

namespace StonkBot.StonkBot.Services.TDAmeritrade.Models
{
    public class AuthToken
    {
        #pragma warning disable IDE1006 // Naming Styles
        [Key]public Guid PrimaryKey { get; set; } = Guid.NewGuid();
        public long tokenCreatedTime { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();
        public string access_token { get; set; } = null!;
        public string refresh_token { get; set; } = null!;
        public string token_type { get; set; } = null!;
        public int expires_in { get; set; }
        public string scope { get; set; } = null!;
        public int refresh_token_expires_in { get; set; }
        #pragma warning restore IDE1006 // Naming Styles
    }
}
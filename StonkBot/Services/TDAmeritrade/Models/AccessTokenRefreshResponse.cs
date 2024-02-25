using System.ComponentModel.DataAnnotations;

namespace StonkBot.StonkBot.Services.TDAmeritrade.Models
{

    public class AccessTokenRefreshResponse
    {
        #pragma warning disable IDE1006 // Naming Styles
        public long tokenCreatedTime { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();
        public string access_token { get; set; } = null!;
        public string scope { get; set; } = null!;
        public int expires_in { get; set; }
        public string token_type { get; set; } = null!;
        #pragma warning restore IDE1006 // Naming Styles
    }
}
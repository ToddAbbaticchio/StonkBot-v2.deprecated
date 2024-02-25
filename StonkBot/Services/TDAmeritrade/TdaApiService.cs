using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using StonkBot.StonkBot._SBResources;
using StonkBot.StonkBot.Database;
using StonkBot.StonkBot.Services.TDAmeritrade.Models;

namespace StonkBot.StonkBot.Services.TDAmeritrade;

internal interface ITdaApiService
{
    Task<string> GetTokenAsync(bool force, CancellationToken cToken);
    Task<Quote?> GetQuoteAsync(string symbol, CancellationToken cToken);
    Task<List<Quote?>> GetQuotesAsync(List<string> symbols, CancellationToken cToken);
    Task<HistoricalData?> GetHistoricalDataAsync(string symbol, string periodType, string period, string frequencyType, CancellationToken cToken);
}

class TdaApiService : ITdaApiService
{
    private readonly ILogger<TdaApiService> _logger;
    private readonly IUrlBuilder _urlBuilder;
    private readonly StonkBotDbContext _db;

    public TdaApiService(ILogger<TdaApiService> logger, IUrlBuilder urlBuilder, StonkBotDbContext db)
    {
        _logger = logger;
        _urlBuilder = urlBuilder;
        _db = db;
    }

    public async Task<string> GetTokenAsync(bool force, CancellationToken cToken)
    {
        try
        {
            // Get first token in DB - if still valid return that token
            var dbToken = await _db.AuthTokens.FirstAsync(cToken);
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            var refreshTime = dbToken.tokenCreatedTime + dbToken.expires_in - 300;
            if (force == false && (dbToken.expires_in != 0 || refreshTime > now))
                return dbToken.access_token;

            // If not valid get a new token using the dbToken refreshtoken
            RestResponse response;
            try
            {
                using var client = new RestClient(Constants.tdTokenUrl);
                var request = new RestRequest() { Method = Method.Post };
                var requestParams = new { grant_type = "refresh_token", refresh_token = dbToken.refresh_token, client_id = Constants.tdAmeritradeClientId };
                request.AddObject(requestParams);
                response = await client.ExecuteAsync(request, cToken);
                if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                    throw new Exception($"Failed to acquire a new TDA access token: {response.ResponseStatus}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error acquiring new access token from TDA API: {ex.Message}");
            }

            // Deserialize the TDA response and store new token info
            try
            {
                var newAuthTokenInfo = JsonConvert.DeserializeObject<AccessTokenRefreshResponse>(response.Content);
                if (newAuthTokenInfo == null || string.IsNullOrEmpty(newAuthTokenInfo.access_token))
                    throw new Exception("Failed to deserialize tdameritrade authtoken response!");

                dbToken.tokenCreatedTime = newAuthTokenInfo.tokenCreatedTime;
                dbToken.access_token = newAuthTokenInfo.access_token;
                dbToken.expires_in = newAuthTokenInfo.expires_in;
                dbToken.scope = newAuthTokenInfo.scope;
                dbToken.token_type = newAuthTokenInfo.token_type;
                await _db.SaveChangesAsync(cToken);

                return newAuthTokenInfo.access_token;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving new access token: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("TdaApiService.GetTokenAsync: {ex}", ex.Message);
            throw new Exception(ex.Message);
        }
    }

    public async Task<Quote?> GetQuoteAsync(string symbol, CancellationToken cToken)
    {
        try
        {
            for (var i = 1; i <= 3; i++)
            {
                var token = await GetTokenAsync(false, cToken);

                using var client = new RestClient(_urlBuilder.GetQuoteUrl(symbol));
                client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");
                var request = new RestRequest() { Method = Method.Get };
                var response = await client.ExecuteGetAsync(request, cToken);
                if (!response.IsSuccessful)
                {
                    if (response.ErrorException != null && response.ErrorException.ToString().Contains("Unauthorized"))
                    {
                        _logger.LogDebug("Attempt {count} of 3 - TDAApi call unauthorized. Expiring TDAToken...", i);
                        await ExpireTokenAsync(cToken);
                        continue;
                    }

                    _logger.LogWarning("Failed to acquire quote response for {symbol} from tdaApi: {ex}", symbol, response.ErrorException);
                }

                if (string.IsNullOrEmpty(response.Content))
                {
                    _logger.LogWarning("Attempt {count} of 3 - Quote response.Content for {symbol} is null or empty!", i, symbol);
                    continue;
                }

                try
                {
                    var quote = JsonConvert.DeserializeObject<QuoteList>(response.Content);
                    if (quote?.Info == null)
                        throw new Exception($"Failed to deserialize quote response for symbol {symbol}!");
                    if (!quote.Info.Any())
                        throw new Exception($"Deserialized quote response for symbol {symbol} is null or empty!");

                    return quote.Info!.FirstOrDefault()!;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Attempt {count} of 3 - Error deserializing response: {err}", i, ex.Message);
                }
            }

            throw new Exception("You shouldn't see this.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Error retrieving quote for {symbol}: {errMsg}", symbol, ex.Message);
            throw;
        }
    }

    public async Task<List<Quote?>> GetQuotesAsync(List<string> symbols, CancellationToken cToken)
    {
        var token = await GetTokenAsync(false, cToken);

        try
        {
            using var client = new RestClient(_urlBuilder.GetQuotesUrl(symbols));
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");
            var request = new RestRequest() { Method = Method.Get };
            var response = await client.ExecuteGetAsync(request, cToken);
            if (!response.IsSuccessful)
                throw new Exception($"Failed to acquire multi-quote response from tdaApi: {response.ErrorMessage}");
            if (string.IsNullOrEmpty(response.Content))
                throw new Exception($"Multi-quote response.Content is null or empty!");

            var quotes = JsonConvert.DeserializeObject<QuoteList>(response.Content);
            if (quotes == null || quotes.Info == null)
                throw new Exception($"Failed to deserialize multi quote response!");
            if (!quotes.Info.Any())
                throw new Exception($"Deserialized multi quote response is null or empty!");

            return quotes.Info!;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Error acquiring multi stonk quote: {errMsg}", ex.Message);
            throw;
        }
    }

    public async Task<HistoricalData?> GetHistoricalDataAsync(string symbol, string periodType, string period, string frequencyType, CancellationToken cToken)
    {
        for (var i = 1; i <= 3; i++)
        {
            var token = await GetTokenAsync(false, cToken);

            var urlTest = _urlBuilder.GetHistoricalQuoteUrl(symbol, periodType, period, frequencyType);
            using var client = new RestClient(urlTest);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");
            var request = new RestRequest() { Method = Method.Get };
            var response = await client.ExecuteGetAsync(request, cToken);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _logger.LogWarning("{symbol} attempt {tryCount} of 3 - Exceeded allowed requests per minute. Pausing 20 seconds and trying again...", symbol, i);
                    Thread.Sleep(20000);
                    continue;
                }

                if (response.Content != null && response.Content.Contains("The access token being passed has expired or is invalid."))
                {
                    await ExpireTokenAsync(cToken);
                    continue;
                }

                if (string.IsNullOrEmpty(response.Content))
                {
                    _logger.LogWarning("{symbol} attempt {tryCount} of 3 - Response was successful, but TDA returned an empty data table. Retrying in 5 seconds...", symbol, i);
                    Thread.Sleep(5000);
                    continue;
                }

                _logger.LogWarning("{symbol} attempt {tryCount} of 3 - GetHistoricalDataAsync unsuccessful. Retrying in 5 seconds...", symbol, i);
                Thread.Sleep(5000);
            }

            var history = JsonConvert.DeserializeObject<HistoricalData>(response.Content);
            if (history == null || !history.candles.Any())
            {
                _logger.LogWarning("HistoricalQuoteRequest for {symbol} yielded no result!", symbol);
                continue;
            }

            return history;
        }

        return null;
    }

    private async Task ExpireTokenAsync(CancellationToken cToken)
    {
        var dbToken = await _db.AuthTokens.FirstAsync(cToken);
        dbToken.expires_in = 0;
        await _db.SaveChangesAsync(cToken);
    }
}






/* MAYBE GET NEW REFRESH TOKEN WITH THIS?
 * 
 * public static class TDAmeritradeService
{
    private const string AuthUrl = "https://api.tdameritrade.com/v1/oauth2/token";
    /// <param name="clientId"></param>
    /// <param name="redirectUri"></param>

    public static void GetAuthCode(string clientId, string redirectUri)
    {
        Process.Start(ChromeAppFileName, $"https://auth.tdameritrade.com/oauth?client_id={clientId}@AMER.OAUTHAP&response_type=code&redirect_uri={redirectUri}");
    }

    public static AuthToken GetAccessToken(string clientId, string code, string redirectUri)
    {
        var client = new RestClient(AuthUrl);
        //var request = new RestRequest(resource, Method.Post);
        request.AddParameter("grant_type", "authorization_code");
        request.AddParameter("client_id", clientId);
        request.AddParameter("access_type", "offline");
        request.AddParameter("code", code);
        request.AddParameter("redirect_uri", redirectUri);
        var response = client.Execute(request);
        return JsonConvert.DeserializeObject<AuthToken>(response.Content);
    }

    private const string ChromeAppKey = @"\Software\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe";
    private static string ChromeAppFileName => (string)(Registry.GetValue("HKEY_LOCAL_MACHINE" + ChromeAppKey, "", null) ?? Registry.GetValue("HKEY_CURRENT_USER" + ChromeAppKey, "", null));
}*/

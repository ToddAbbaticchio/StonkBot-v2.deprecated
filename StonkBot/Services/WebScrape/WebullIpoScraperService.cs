using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using StonkBot.StonkBot._SBResources;
using StonkBot.StonkBot.Services.WebScrape.Models;

namespace StonkBot.StonkBot.Services.WebScrape
{
    internal interface IWebullIpoScraperService
    {
        Task<List<IpoListing>> ScrapeAsync(CancellationToken cToken);
    }
    class WebullIpoScraperService : IWebullIpoScraperService
    {
        private readonly IWebullIpoListingFactory _listingFactory;
        private readonly HttpClient _httpClient;
        private readonly HtmlDocument _htmlDocument;
        private readonly ILogger<WebullIpoScraperService> _logger;

        public WebullIpoScraperService(HttpClient httpClient, HtmlDocument htmlDocument, IWebullIpoListingFactory listingFactory, ILogger<WebullIpoScraperService> logger)
        {
            _listingFactory = listingFactory;
            _httpClient = httpClient;
            _htmlDocument = htmlDocument;
            _logger = logger;
        }

        public async Task<List<IpoListing>> ScrapeAsync(CancellationToken cToken)
        {
            try
            {
                // scrape page
                HttpResponseMessage response = await _httpClient.GetAsync(Constants.webullScrapeUrl, cToken);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync(cToken);

                // process html data
                _htmlDocument.LoadHtml(result);
                var docNodes = _htmlDocument.DocumentNode.DescendantsAndSelf().ToList();
                var scrapedIpos = _listingFactory.Process(docNodes);

                return scrapedIpos;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {errMsg}", ex.Message);
                return new List<IpoListing>();
            }
        }
    }
}
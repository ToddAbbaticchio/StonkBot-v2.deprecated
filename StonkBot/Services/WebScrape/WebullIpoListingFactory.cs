using HtmlAgilityPack;
using StonkBot.StonkBot.Services.WebScrape.Models;

namespace StonkBot.StonkBot.Services.WebScrape
{
    internal interface IWebullIpoListingFactory
    {
        List<IpoListing> Process(List<HtmlNode> htmlNodes);
    }

    public class WebullIpoListingFactory : IWebullIpoListingFactory
    {
        public List<IpoListing> Process(List<HtmlNode> htmlNodes)
        {
            var ipoList = new List<IpoListing>();
            var node = 0;
            foreach (var docNode in htmlNodes)
            {
                if (docNode is { Name: "a", NextSibling: { Name: "p" } })
                {
                    List<HtmlNode> subList = htmlNodes.GetRange(node, 27);
                    subList.RemoveAll(x => x.Name == "p");
                    subList.RemoveAll(x => x.Name == "span");
                    subList.RemoveAll(x => x.Name == "div");

                    var currListing = new IpoListing
                    {
                        Symbol = subList[1].InnerHtml,
                        Name = subList[2].InnerHtml,
                        OfferingPrice = subList[3].InnerHtml,
                        OfferAmmount = subList[4].InnerHtml,
                        OfferingEndDate = subList[5].InnerHtml,
                        ExpectedListingDate = subList[6].InnerHtml,
                        ScrapeDate = DateTime.Today.ToString("MM/dd/yy")
                    };
                    ipoList.Add(currListing);
                }
                node++;
            }

            return ipoList;
        }
    }
}
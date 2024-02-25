using System.Diagnostics;
using System.Xml.Linq;
using Discord.Webhook;
using StonkBot.StonkBot._SBResources;
using StonkBot.StonkBot.Actions.Enums;
using StonkBot.StonkBot.Database;
using StonkBot.StonkBot.Database.Entities.IndustryAnalysis;
using StonkBot.StonkBot.Database.Enums;
using StonkBot.StonkBot.Services.TDAmeritrade;
using StonkBot.StonkBot.Services.WebScrape;
using StonkBot.StonkBot.Utilities;

namespace StonkBot.StonkBot.Actions
{
    public interface IStonkBotActions
    {
        Task IpoCheck(CancellationToken cToken);
        Task IpoScrape(CancellationToken cToken);
        Task SendDiscordAlerts(CancellationToken cToken);
        Task UpdateAllCalculatedFields(bool recalcAll, CancellationToken cToken);
        Task UpdateIndustryAnalysisStocks(CancellationToken cToken);
    }

    internal class StonkBotActions : IStonkBotActions
    {
        private readonly ITdaApiService _tdaService;
        private readonly IWebullIpoScraperService _webullScraperService;
        private readonly ISpecialActions _specialActions;
        private readonly ILogger<StonkBotActions> _logger;
        private readonly IStonkBotDb _db;
        private readonly ConsoleWriter _cWriter; 

        public StonkBotActions(IStonkBotDb db, ITdaApiService tdaService, IWebullIpoScraperService webullScraperService, ISpecialActions specialActions, ILogger<StonkBotActions> logger, ConsoleWriter cWriter)
        {
            _db = db;
            _tdaService = tdaService;
            _webullScraperService = webullScraperService;
            _logger = logger;
            _cWriter = cWriter;
            _specialActions = specialActions;
        }

        public async Task IpoScrape(CancellationToken cToken)
        {
            try
            {
                var ipoList = await _webullScraperService.ScrapeAsync(cToken);
                if (!ipoList.Any())
                {
                    _logger.LogWarning("webullScraperService.ScrapeAsync returned nothing. Is a Problem?");
                    return;
                }

                //using var db = new StonkBotDbContext();
                List<string> logList = new();
                foreach (var ipo in ipoList)
                {
                    var ipoExistState = await _db.IpoExistsAsync(ipo, cToken);
                    switch (ipoExistState)
                    {
                        case EntityState.FullMatch:
                            _logger.LogDebug("The IPO listing for {symbol} listing on {listDate} already exists in the DB. Skipping!",ipo.Symbol, ipo.ExpectedListingDate);
                            break;

                        case EntityState.SymbolMatch:
                            await _db.UpdateIpoAsync(ipo, cToken);
                            logList.Add($"Updated existing entry for {ipo.Symbol}");
                            break;

                        case EntityState.NotExist:
                            await _db.AddIpoAsync(ipo, cToken);
                            logList.Add($"Created new entry for for {ipo.Symbol}");
                            break;
                    }
                }

                var logListString = string.Join(Environment.NewLine, logList);
                if (!string.IsNullOrEmpty(logListString))
                    _logger.LogInformation("{message}", logListString);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during StonkBotActions.IpoScrape: {errMsg}", ex.Message);
            }
        }

        public async Task IpoCheck(CancellationToken cToken)
        {
            try
            {
                var todayDate = DateTime.Now.ToString("MM/dd/yyyy");
                var allIpos = await _db.GetIpoListingsAsync(cToken)!;
                SortedDictionary<string, string> ipoAlertMessages = new();

                foreach (var ipo in allIpos)
                {
                    // Opened today - have not yet record opening day info
                    try
                    {
                        if (ipo.ExpectedListingDate == todayDate && !ipo.OpeningDayInfoRecorded())
                        {
                            var quote = await _tdaService.GetQuoteAsync(ipo.Symbol, cToken);
                            if (quote != null)
                                await _db.UpdateIpoAsync(quote, cToken);
                            _logger.LogInformation("Added opening day info for watched Ipo: {symbol}", ipo.Symbol);
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Opening day check for {ipo.Symbol} exception: {ex.Message}");
                    }

                    // Surpassed opening day close price for the first time
                    try
                    {
                        if (ipo.ExpectedListingDate == todayDate || !ipo.OpeningDayInfoRecorded())
                            continue;

                        var quote = await _tdaService.GetQuoteAsync(ipo.Symbol, cToken);
                        var ipoClose = (decimal)ipo.Close!;
                        var quoteClose = quote!.closePrice;
                        if (quoteClose <= ipoClose)
                            continue;

                        ipoAlertMessages.Add(ipo.Symbol, $"{ipo.Symbol}'s closePrice surpassed openingDay's closePrice!  {todayDate}: [${quote.closePrice}]  OpeningDay: [${ipo.Close}]");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Exceeds opening day check for {ipo.Symbol} exception: {ex.Message}");
                    }
                }

                if (ipoAlertMessages is { Count: > 0 })
                {
                    var volAlert = new DiscordWebhookClient(DiscordConstants.IpoWatchWebHook);
                    string? msgBody = null;
                    foreach (var msg in ipoAlertMessages.Values)
                    {
                        msgBody ??= $"```\r\nIPO Alerts - Date: [{todayDate}]\r\n";
                        msgBody += $"{msg}\r\n";
                        var msgBodyLen = msgBody.Length;
                        if (msgBodyLen < 1900)
                            continue;

                        msgBody += $"```";
                        await volAlert.SendMessageAsync(msgBody);
                        msgBody = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during StonkBotActions.IpoCheck: {errMsg}", ex.Message);
            }
        }

        public async Task UpdateIndustryAnalysisStocks(CancellationToken cToken)
        {
            var todayDay = DateTime.Today.DayOfWeek;
            if (todayDay is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                _cWriter.WriteMessage($"Today is {todayDay} - Can't pull EoD numbers when the market isn't open...", ConsoleColor.DarkRed);
                return;
            }

            var today = DateTime.Today.Date;
            _cWriter.WriteMessage($"Updating EoD numbers for all IndustryAnalysis group symbols...", ConsoleColor.DarkCyan);
            
            // Update targeted stocks daily numbers after market close
            var symbolList = await _db.GetIASymbols(cToken);
            var symbolCount = symbolList.Count;
            var quoteList = new List<IAHistoricalData>();
            
            const int grabSize = 250;
            for (var i = 0; i < symbolList.Count; i += grabSize)
            {
                var subList = symbolList.Skip(i).Take(grabSize).ToList();
                var subListQuotes = await _tdaService.GetQuotesAsync(subList, cToken);
                foreach (var quote in subListQuotes)
                {
                    if (quote == null)
                        continue;

                    var exists = await _db.IAHDataExistsAsync(quote, today, cToken);
                    switch (exists)
                    {
                        case EntityState.NotExist:
                            if (DateTime.Now.TimeOfDay <= new TimeSpan(20, 0, 0))
                                continue;
                            
                            quoteList.Add(new IAHistoricalData
                            {
                                Symbol = quote.symbol!,
                                Open = quote.openPrice,
                                Close = quote.regularMarketLastPrice,
                                Low = quote.lowPrice,
                                High = quote.highPrice,
                                Volume = quote.totalVolume,
                                Datetime = DateTime.Today.Date,
                            });
                            continue;

                        case EntityState.FullMatch:
                            continue;

                        case EntityState.SymbolMatch:
                        default:
                            _cWriter.WriteMessage($"DB returned entityState {exists} for symbol {quote!.symbol} - Look into this!", ConsoleColor.DarkYellow);
                            continue;
                    }
                }
                _cWriter.WriteProgress(i+250, symbolCount);
            }
            _cWriter.WriteProgressComplete("ProcessingComplete!");
            if (!quoteList.Any())
                return;

            _cWriter.WriteMessage($"Adding IAHistoricalData for [{quoteList.Count}] symbols for {today}!", ConsoleColor.DarkCyan);
            await _db.AddIAHDataAsync(quoteList, cToken);
            _cWriter.WriteMessage("Done!", ConsoleColor.DarkCyan);
        }

        public async Task UpdateAllCalculatedFields(bool recalcAll, CancellationToken cToken)
        {
            var testContext = new StonkBotDbContext();
            
            /*var timer0 = new Stopwatch();
            timer0.Start();
            await _specialActions.ParallelTestCalculateField(CalculatedFields.Sector, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.Industry, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.Category, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.FromYesterday, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.UpToday, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.VolumeAlert, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.VolumeAlert2, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.FiveDayStable, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.UpperShadow, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.AboveUpperShadow, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.FhTargetDay, recalcAll, cToken);
            await _specialActions.ParallelTestCalculateField(CalculatedFields.LastFhTarget, recalcAll, cToken);
            timer0.Stop();
            _cWriter.WriteMessage($"Full recalc parallel took: [{timer0.Elapsed}]");
            
            var result1 = testContext.IAHistoricalData.ToList();*/


            var timer1 = new Stopwatch();
            timer1.Start();
            await _specialActions.CalculateField(CalculatedFields.Sector, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.Industry, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.Category, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.FromYesterday, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.UpToday, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.VolumeAlert, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.VolumeAlert2, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.FiveDayStable, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.UpperShadow, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.AboveUpperShadow, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.FhTargetDay, recalcAll, cToken);
            await _specialActions.CalculateField(CalculatedFields.LastFhTarget, recalcAll, cToken);
            timer1.Stop();
            _cWriter.WriteMessage($"Full recalc non parallel took: [{timer1.Elapsed}]");

            /*var result2 = testContext.IAHistoricalData.ToList();

            var test = result1.Equals(result2);
            Console.WriteLine("asdf");*/
        }

        public async Task SendDiscordAlerts(CancellationToken cToken)
        {
            await _specialActions.ScanForAlerts(Alerts.VolumeAlert, DateTime.Today, cToken);
            await _specialActions.ScanForAlerts(Alerts.VolumeAlert2, DateTime.Today, cToken);
            await _specialActions.ScanForAlerts(Alerts.UpperShadowAlert, DateTime.Today, cToken);
            await _specialActions.ScanForAlerts(Alerts.FourHandAlert, DateTime.Today, cToken);
        }
        
        /*var token = await GetTdaAuthTokenAsync(cToken);
        //var quoteList = _tdaService.GetHistoricalDataAsync("AAPL", token, "year", "1", "daily", cToken);
        //var quoteTest = _tdaService.GetQuoteAsync("HUAK", token, cToken);


        var filePath = "C:\\Users\\tabba\\Desktop\\\\etfs.csv";
        //var filePath = "C:\\Users\\tabba\\Desktop\\stock_list\\industries_outFile.csv";

        using (TextFieldParser csvParser = new TextFieldParser(filePath))
        {
            csvParser.CommentTokens = new string[] { "#" };
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = true;

            // Skip the row with the column names
            csvParser.ReadLine();

            while (!csvParser.EndOfData)
            {
                // Read current line fields, pointer moves to the next line.
                string[] data = csvParser.ReadFields();
                Console.WriteLine($"Working on: {data[1]}");
                var thisInfo = new IAEtfInfo
                {
                    Sector = data[0],
                    Symbol = data[1],
                };

                await _db.AddAsync(thisInfo, cToken);
            }

            await _db.SaveChangesAsync(cToken);
            Console.WriteLine("Done!");*/
    }
}
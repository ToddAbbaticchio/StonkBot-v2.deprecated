using Microsoft.EntityFrameworkCore;
using StonkBot.StonkBot.Database.Entities.IndustryAnalysis;
using StonkBot.StonkBot.Services.TDAmeritrade.Models;
using StonkBot.StonkBot.Services.WebScrape.Models;
using EntityState = StonkBot.StonkBot.Database.Enums.EntityState;

namespace StonkBot.StonkBot.Database;

public interface IStonkBotDb
{
    Task<EntityState> IpoExistsAsync(IpoListing ipo, CancellationToken cToken);
    Task<List<IpoListing>>? GetIpoListingsAsync(CancellationToken cToken);
    Task AddIpoAsync(IpoListing ipo, CancellationToken cToken);
    Task UpdateIpoAsync(IpoListing ipo, CancellationToken cToken);
    Task UpdateIpoAsync(Quote quote, CancellationToken cToken);


    Task<List<string>> GetIASymbols(CancellationToken cToken);
    Task<List<IAHistoricalData>> GetAllIAHData(CancellationToken cToken);
    Task<EntityState> IAHDataExistsAsync(Quote quote, DateTime targetDate, CancellationToken cToken);
    Task AddIAHDataAsync(List<IAHistoricalData> quoteList, CancellationToken cToken);
}

public class StonkBotDbContext : DbContext, IStonkBotDb
{
    public string DbPath { get; }
    public DbSet<AuthToken> AuthTokens { get; set; } = null!;
    public DbSet<IpoListing> IpoListings { get; set; } = null!;
    public DbSet<IAHistoricalData> IAHistoricalData { get; set; } = null!;
    public DbSet<IAInfo> IAInfo { get; set; } = null!;
    public DbSet<IAEtfInfo> IAEtfInfo { get; set; } = null!;
    public StonkBotDbContext()
    {
        const string path = "E:\\projects\\stonkBot\\Data";
        DbPath = Path.Join(path, "StonkBotService_Beta.db");
    }
    
    #region Ipo Methods
    public async Task<EntityState> IpoExistsAsync(IpoListing ipo, CancellationToken cToken)
    {
        var checkFullMatch = await IpoListings
            .Where(x => x.ExpectedListingDate == ipo.ExpectedListingDate)
            .Where(x => x.OfferingEndDate == ipo.OfferingEndDate)
            .Where(x => x.OfferingPrice == ipo.OfferingPrice)
            .Where(x => x.OfferAmmount == ipo.OfferAmmount)
            .AsSingleQuery()
            .AnyAsync(cToken);
        if (checkFullMatch)
            return Enums.EntityState.FullMatch;

        var checkSymbolMatch = await IpoListings
            .Where(x => x.Symbol == ipo.Symbol)
            .AnyAsync(cToken);
        if (checkSymbolMatch)
            return Enums.EntityState.SymbolMatch;

        return Enums.EntityState.NotExist;
    }

    public async Task<List<IpoListing>>? GetIpoListingsAsync(CancellationToken cToken)
    {
        return await IpoListings.ToListAsync(cToken);
    }

    public async Task AddIpoAsync(IpoListing ipo, CancellationToken cToken)
    {
        await using var saveDb = new StonkBotDbContext();
        await saveDb.IpoListings.AddAsync(ipo, cToken);
        await saveDb.SaveChangesAsync(cToken);
    }

    public async Task UpdateIpoAsync(IpoListing ipo, CancellationToken cToken)
    {
        var updatedIpos = await IpoListings
            .Where(x => x.Symbol == ipo.Symbol)
            .ExecuteUpdateAsync(o => o
                .SetProperty(x => x.ExpectedListingDate, ipo.ExpectedListingDate)
                .SetProperty(x => x.OfferAmmount, ipo.OfferAmmount)
                .SetProperty(x => x.OfferingPrice, ipo.OfferingPrice)
                .SetProperty(x => x.OfferingEndDate, ipo.OfferingEndDate),
            cToken);

        if (updatedIpos != 1)
            Console.WriteLine($"UpdateIpoAsync should have only changed 1 row, but {updatedIpos} rows were effected. This is probably bad.");
    }

    public async Task UpdateIpoAsync(Quote quote, CancellationToken cToken)
    {
        var updatedIpos = await IpoListings
            .Where(x => x.Symbol == quote.symbol)
            .ExecuteUpdateAsync(o => o
                .SetProperty(x => x.Open, quote.openPrice)
                .SetProperty(x => x.Close, quote.regularMarketLastPrice)
                .SetProperty(x => x.Low, quote.lowPrice)
                .SetProperty(x => x.High, quote.highPrice)
                .SetProperty(x => x.Volume, quote.totalVolume),
            cToken);

        if (updatedIpos != 1)
            Console.WriteLine($"UpdateIpoAsync should have only changed 1 row, but {updatedIpos} rows were effected. This is probably bad.");
    }
    #endregion

    #region IA Methods
    public async Task<List<string>> GetIASymbols(CancellationToken cToken)
    {
        var iaInfos = await IAInfo
            .Select(x => x.Symbol)
            .ToListAsync(cToken);
        
        var etfs = await IAEtfInfo
            .Select(x => x.Symbol)
            .ToListAsync(cToken);

        var combinedList = new List<string>();
        combinedList.AddRange(iaInfos);
        combinedList.AddRange(etfs);

        return combinedList.Distinct().ToList();
    }

    public async Task<List<IAHistoricalData>> GetAllIAHData(CancellationToken cToken)
    {
        return await IAHistoricalData.ToListAsync(cToken);
    }

    public async Task<EntityState> IAHDataExistsAsync(Quote quote, DateTime targetDate, CancellationToken cToken)
    {
        var checkFullMatch = await IAHistoricalData
            .Where(x => x.Symbol == quote.symbol)
            .Where(x => x.Datetime == targetDate.Date)
            .AsSingleQuery()
            .AnyAsync(cToken);
        
        return checkFullMatch ? EntityState.FullMatch : EntityState.NotExist;
    }

    public async Task AddIAHDataAsync(List<IAHistoricalData> quoteList, CancellationToken cToken)
    {
        await using var saveDb = new StonkBotDbContext();
        await saveDb.IAHistoricalData.AddRangeAsync(quoteList, cToken);
        await saveDb.SaveChangesAsync(cToken);
    }
    #endregion



    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IAInfo>(e => e.HasKey(x => x.Symbol));
        modelBuilder.Entity<IAEtfInfo>(e => e.HasKey(x => x.Symbol));

        modelBuilder.Entity<IAHistoricalData>(e =>
        {
            e.HasKey(x => new { x.Symbol, x.Datetime });
        });
    }
}
using StonkBot.StonkBot.Database.Entities.IndustryAnalysis;
using System.Collections.Concurrent;
using System.Diagnostics;
using StonkBot.StonkBot.Database;
using StonkBot.StonkBot.Utilities;

namespace StonkBot.StonkBot.Actions;

public partial interface ICalculateField
{
    Task Industry(bool recalculateAll, CancellationToken cToken);
}

internal partial class Calculate
{
    public async Task Industry(bool recalculateAll, CancellationToken cToken)
    {
        var _db = new StonkBotDbContext();
        var _cWriter = new ConsoleWriter();

        var hData = _db.IAHistoricalData;
        var missingData = recalculateAll
            ? hData.Take(250000)
            : hData
                .Where(x => string.IsNullOrEmpty(x.Sector));

        _cWriter.WriteMessage(!missingData.Any() ? "No DB entries found missing Industry field!" : "Processing DB entries missing Industry field...", ConsoleColor.DarkCyan);
        if (!missingData.Any())
            return;

        var timer = new Stopwatch();
        timer.Start();
        var updatedData = new ConcurrentBag<IAHistoricalData>();
        ParallelOptions _pOptions = new() { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 };
        Parallel.ForEach(missingData, _pOptions, async entry =>
        {
            var updatedEntry = await GetIndustryAsync(entry, cToken);
            if (updatedEntry != null)
                updatedData.Add(updatedEntry);
        });
        
        if (!updatedData.IsEmpty)
        {
            _cWriter.WriteMessage($"Processed {missingData.Count()} entries missing Industry field. Committing changes to DB...", textColor: ConsoleColor.DarkCyan);
            _db.UpdateRange(updatedData);
            
            timer.Stop();
            _cWriter.WriteMessage($"Done! Elapsed time: [{timer.Elapsed}] Averaging [{Convert.ToSingle(timer.ElapsedMilliseconds) / Convert.ToSingle(updatedData.Count)}]ms per entry updated!", textColor: ConsoleColor.DarkCyan);
            return;
        }

        timer.Stop();
        _cWriter.WriteMessage($"Found {missingData.Count()} entries missing Industry field in [{timer.Elapsed}] but none were fixable.", ConsoleColor.DarkCyan);
    }

    private static async Task<IAHistoricalData?> GetIndustryAsync(IAHistoricalData entry, CancellationToken cToken)
    {
        var _db = new StonkBotDbContext();
        var _cWriter = new ConsoleWriter();

        var iaInfo = _db.IAInfo;
        var etfInfo = _db.IAEtfInfo;

        try
        {
            var iaMatch = await iaInfo.FindAsync(new object?[] { entry.Symbol }, cToken);
            if (iaMatch != null)
            {
                entry.Industry = iaMatch.Industry.Replace(" ", "").Replace("-", "").Replace(",", "");
                return entry;
            }

            var etfMatch = await etfInfo.FindAsync(new object?[] { entry.Symbol }, cToken);
            if (etfMatch != null)
            {
                entry.Industry = "ETF";
                return entry;
            }

            entry.Industry = "missingdata";
            return entry;
        }
        catch (Exception ex)
        {
            _cWriter.WriteMessage($"Error processing {entry.Symbol}-{entry.Datetime} for missing Industry field: {ex.Message}");
            return null;
        }
    }
}
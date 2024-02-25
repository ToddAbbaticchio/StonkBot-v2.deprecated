using StonkBot.StonkBot.Database;
using StonkBot.StonkBot.Services.TDAmeritrade;
using StonkBot.StonkBot.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StonkBot.StonkBot.Actions;

public partial interface ICalculateField
{

}

internal partial class Calculate : ICalculateField
{
    public Calculate()
    {
        //_db = db;
        //_tdaService = tdaService;
        //_cWriter = cWriter;
    }

    public async Task All(bool recalculateAll, CancellationToken cToken)
    {

    }
}

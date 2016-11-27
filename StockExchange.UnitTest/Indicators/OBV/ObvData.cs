using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.Helpers;
using System.Collections.Generic;

namespace StockExchange.UnitTest.Indicators.OBV
{
    public class ObvData
    {
        public const int DataPrecision = 6;

        internal static IList<Price> HistoricalData => DataHelper.ConvertToPrices(new[,]
        {
            {58.69m,   57.41m,   57.82m,   12981400 },
            {57.7m,    56.77m,   56.82m,   10207900 },
            {58.5m,    57.69m,   58.3m,    6835200  },
            {58.07m,   56.15m,   56.62m,   7111900  },
            {57.85m,   56.52m,   57.27m,   5574000  },
            {57.81m,   56.22m,   57.55m,   5694600  },
            {58.19m,   57.47m,   57.58m,   5012600  },
            {58.91m,   58.07m,   58.53m,   4688300  },
            {58.43m,   57.45m,   58.43m,   4113700  },
            {60m,      59.61m,   59.93m,   6039100  },
            {59.24m,   58.61m,   59.13m,   5199500  },
            {58.97m,   58.02m,   58.33m,   4827800  },
            {58.12m,   57.15m,   57.27m,   4380100  },
            {58.41m,   57.55m,   58.35m,   3751100  },
            {59.66m,   58.95m,   59.26m,   4582900  },
            {59.58m,   59.01m,   59.5m,    3780600  },
            {59.63m,   58.98m,   59.24m,   2838200  },
            {58.71m,   57.76m,   58.63m,   3857900  },
            {57.86m,   57.33m,   57.83m,   3338000  },
            {59.09m,   58.46m,   58.64m,   6127400  }
        });

        internal static IList<IndicatorValue> ExpectedResults => DataHelper.ConvertToIndicatorValues(new[]
        {
            12981400m,
             2773500m,
             9608700m,
             2496800m,
             8070800m,
            13765400m,
            18778000m,
            23466300m,
            19352600m,
            25391700m,
            20192200m,
            15364400m,
            10984300m,
            14735400m,
            19318300m,
            23098900m,
            20260700m,
            16402800m,
            13064800m,
            19192200m,
        });
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Indicators
{
    /// <summary>
    /// Wskaźnik Macd
    /// </summary>
    public class MacdIndicator : BaseIndicator
    {
        public MacdIndicator() : base()
        {
        }

        public MacdIndicator(IRepository<Price> priceRepository) : base(priceRepository)
        {
        }

        public override IList<decimal> Calculate(IList<Price> historicalPrices)
        {
            throw new NotImplementedException();
        }
    }
}

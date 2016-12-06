using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Models;

namespace StockExchange.Business.Services
{
    public interface IStrategyService
    {
        void CreateStrategy(StrategyDto strategy);
    }
}

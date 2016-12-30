using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockExchange.Web.Models.Wallet
{
    public class BudgetInfoViewModel
    {
        public decimal FreeBudget { get; set; }

        public decimal AllStocksValue { get; set; }

        public decimal TotalBudget => FreeBudget + AllStocksValue;
    }
}
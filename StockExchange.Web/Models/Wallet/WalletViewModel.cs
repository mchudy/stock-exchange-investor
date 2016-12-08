using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockExchange.Business.Models;

namespace StockExchange.Web.Models.Wallet
{
    public class WalletViewModel
    {
        public decimal Budget { get; set; }

        public IList<UserTransactionDto> Transactions { get; set; }
    }
}
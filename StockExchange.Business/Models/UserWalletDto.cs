using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Models
{
    public class UserWalletDto
    {
        public decimal Budget { get; set; }
        public IList<UserTransactionDto> Transactions { get; set; }
    }
}

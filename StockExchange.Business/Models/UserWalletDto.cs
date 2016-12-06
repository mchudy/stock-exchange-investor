using System.Collections.Generic;

namespace StockExchange.Business.Models
{
    public class UserWalletDto
    {
        public decimal Budget { get; set; }

        public IList<UserTransactionDto> Transactions { get; set; }
    }
}

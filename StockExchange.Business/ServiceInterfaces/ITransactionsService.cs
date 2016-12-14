using StockExchange.Business.Models;
using System.Collections.Generic;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ITransactionsService
    {
        bool AddUserTransaction(UserTransactionDto dto);
        IList<UserTransactionDto> GetUserTransactions(int userId);
        int GetUserTransactionsCount(int userId);
    }
}
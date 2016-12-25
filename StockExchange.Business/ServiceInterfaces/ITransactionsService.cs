using StockExchange.Business.Models;
using System.Collections.Generic;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ITransactionsService
    {
        void AddUserTransaction(UserTransactionDto dto);

        IList<UserTransactionDto> GetUserTransactions(int userId);

        int GetUserTransactionsCount(int userId);

        Dictionary<int, List<UserTransaction>> GetTransactionsByCompany(int userId);
    }
}
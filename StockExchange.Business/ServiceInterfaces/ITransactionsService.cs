using System.Collections.Generic;
using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Transaction;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ITransactionsService
    {
        void AddUserTransaction(UserTransactionDto dto);

        PagedList<UserTransactionDto> GetUserTransactions(int userId, PagedFilterDefinition<TransactionFilter> filter);

        int GetUserTransactionsCount(int userId);

        Dictionary<int, List<UserTransaction>> GetTransactionsByCompany(int userId);
    }
}
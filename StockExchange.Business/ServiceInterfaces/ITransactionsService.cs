using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Transaction;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ITransactionsService
    {
        Task AddTransaction(UserTransactionDto dto);

        Task<PagedList<UserTransactionDto>> GetTransactions(int userId, PagedFilterDefinition<TransactionFilter> filter);

        Task<int> GetTransactionsCount(int userId);

        Task<Dictionary<int, List<UserTransaction>>> GetTransactionsByCompany(int userId);
    }
}
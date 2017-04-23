using StockExchange.Business.ErrorHandling;
using StockExchange.Business.Exceptions;
using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Paging;
using StockExchange.Business.Models.Transaction;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides methods for operating on user transactions
    /// </summary>
    public class TransactionsService : ITransactionsService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionsRepository _transactionsRepository;

        /// <summary>
        /// Creates a new instance of <see cref="TransactionsService"/>
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="transactionsRepository"></param>
        public TransactionsService(IUserRepository userRepository, ITransactionsRepository transactionsRepository)
        {
            _userRepository = userRepository;
            _transactionsRepository = transactionsRepository;
        }

        /// <inheritdoc />
        public async Task<PagedList<UserTransactionDto>> GetTransactions(int userId, PagedFilterDefinition<TransactionFilter> filter)
        {
            var allTransactions = await _transactionsRepository.GetAllUserTransactions(userId);
            var transactions = allTransactions.Select(t => new UserTransactionDto
                {
                    Date = t.Date,
                    Price = t.Price,
                    Quantity = t.Quantity < 0 ? -t.Quantity : t.Quantity,
                    Action = t.Quantity < 0 ? Action.Sell : Action.Buy,
                    Id = t.Id,
                    CompanyName = t.Company.Code,
                    Total = t.Quantity < 0 ? -t.Quantity * t.Price : t.Quantity * t.Price,
                    Profit = 0
                }).ToList();

            foreach (var pagedTransaction in transactions)
            {
                if (pagedTransaction.Action == Action.Buy) continue;
                var pastTransactions = transactions
                    .Where(item => item.CompanyName == pagedTransaction.CompanyName && item.Date < pagedTransaction.Date)
                    .OrderBy(item => item.Date)
                    .ToList();
                var quantity = 0m;
                var price = 0m;
                foreach (var pastTransaction in pastTransactions)
                {
                    if (pastTransaction.Action == Action.Buy)
                    {
                        price = (price * quantity + pastTransaction.Price * pastTransaction.Quantity) /
                                (quantity + pastTransaction.Quantity);
                        quantity += pastTransaction.Quantity;
                    }
                    else
                    {
                        quantity -= pastTransaction.Quantity;
                    }
                }
                pagedTransaction.Profit = pagedTransaction.Quantity * (pagedTransaction.Price - price);
            }


            if (!string.IsNullOrWhiteSpace(filter.Search))
                transactions = transactions.Where(item => item.CompanyName.Contains(filter.Search.ToUpper())).ToList();

            transactions = transactions.OrderBy(filter.OrderBys).ToList();

            return transactions.ToPagedList(filter.Start, filter.Length);
        }

        /// <inheritdoc />
        public async Task<int> GetTransactionsCount(int userId)
        {
            return await _transactionsRepository.GetTransactionsCount(userId);
        }

        /// <inheritdoc />
        public async Task AddTransaction(UserTransactionDto dto)
        {
            var user = await _userRepository.GetUserWithTransactions(dto.UserId);
            if (user == null)
                throw new BusinessException(nameof(dto.UserId), "User does not exist", ErrorStatus.DataNotFound);
            VerifyTransaction(dto, user);
            user.Budget -= dto.Quantity * dto.Price;
            user.Transactions.Add(new UserTransaction
            {
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                Date = dto.Date,
                Price = dto.Price,
                Quantity = dto.Quantity
            });
            var saveTask = _userRepository.Save();
            var clearCache = _transactionsRepository.ClearTransactionsCache(dto.UserId);
            await Task.WhenAll(saveTask, clearCache);
        }


        /// <inheritdoc />
        public async Task DeleteTransaction(int id, int sessionUserId)
        {
            var transaction = await _transactionsRepository.GetTransaction(id);
            if(transaction == null)
                throw new BusinessException("The transaction does not exist", ErrorStatus.DataNotFound);
            if(transaction.UserId != sessionUserId)
                throw new BusinessException("No permissions to perform this action", ErrorStatus.AccessDenied);

            var user = await _userRepository.GetUser(sessionUserId);
            if(user == null)
                throw new BusinessException("The user does not exist", ErrorStatus.DataNotFound);

            var value = transaction.Quantity*transaction.Price;
            user.Budget += value;
            await _userRepository.Save();

            _transactionsRepository.Remove(transaction);
            var saveTransactionTask =_transactionsRepository.Save();
            var clearCacheTask = _transactionsRepository.ClearTransactionsCache(sessionUserId);

            await Task.WhenAll(saveTransactionTask, clearCacheTask);
        }
        
        /// <inheritdoc />
        public async Task<Dictionary<Company, List<UserTransaction>>> GetTransactionsByCompany(int userId)
        {
            return await _transactionsRepository.GetTransactionsByCompany(userId);
        }

        private static void VerifyTransaction(UserTransactionDto dto, User user)
        {
            if (dto.Quantity < 0)
            {
                int currentlyOwnedStocksCount = user.Transactions.Where(t => t.CompanyId == dto.CompanyId)
                    .Sum(t => t.Quantity);
                if (currentlyOwnedStocksCount < -dto.Quantity)
                {
                    throw new BusinessException($"Cannot sell more stocks than currently owned ({currentlyOwnedStocksCount})");
                }
            }
            else if (user.Budget < dto.Price * dto.Quantity)
            {
                throw new BusinessException($"Not enough free budget (currently {user.Budget})");
            }
        }
    }
}


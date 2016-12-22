using StockExchange.Business.ErrorHandling;
using StockExchange.Business.Exceptions;
using StockExchange.Business.Models;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StockExchange.Business.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserTransaction> _transactionsRepository;

        public TransactionsService(IRepository<User> userRepository, IRepository<UserTransaction> transactionsRepository)
        {
            _userRepository = userRepository;
            _transactionsRepository = transactionsRepository;
        }

        public IList<UserTransactionDto> GetUserTransactions(int userId)
        {
            return _transactionsRepository.GetQueryable()
                .Include(t => t.Company)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .Select(t => new UserTransactionDto
                {
                    Company = t.Company,
                    UserId = t.UserId,
                    Date = t.Date,
                    Price = t.Price,
                    Quantity = t.Quantity
                }).ToList();
        }

        public int GetUserTransactionsCount(int userId)
        {
            return _transactionsRepository.GetQueryable()
                .Count(t => t.UserId == userId);
        }

        public void AddUserTransaction(UserTransactionDto dto)
        {
            var user = _userRepository.GetQueryable()
                .Include(u => u.Transactions)
                .FirstOrDefault(u => u.Id == dto.UserId);

            if (user == null)
                throw new BusinessException(nameof(dto.UserId), "User does not exist", ErrorStatus.DataNotFound);

            VerifyTransaction(dto, user);

            user.Budget -= dto.Quantity * dto.Price;
            user.Transactions.Add(new UserTransaction
            {
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                Date = DateTime.Now,
                Price = dto.Price,
                Quantity = dto.Quantity
            });
            _userRepository.Save();
        }

        private static void VerifyTransaction(UserTransactionDto dto, User user)
        {
            if (dto.Quantity < 0)
            {
                int currentlyOwnedStocksCount =user.Transactions.Where(t => t.CompanyId == dto.CompanyId)
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

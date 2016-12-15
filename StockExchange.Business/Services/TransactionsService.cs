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

        //TODO: return validation messages
        public bool AddUserTransaction(UserTransactionDto dto)
        {
            var user = _userRepository.GetQueryable()
                .Include(u => u.Transactions)
                .FirstOrDefault(u => u.Id == dto.UserId);
            if (user == null)
                return false;

            if (!IsTransactionValid(dto, user))
                return false;

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
            return true;
        }

        private static bool IsTransactionValid(UserTransactionDto dto, User user)
        {
            if (dto.Quantity < 0)
            {
                if (user.Transactions.Where(t => t.CompanyId == dto.CompanyId).Sum(t => t.Quantity) < -dto.Quantity)
                {
                    return false;
                }
            }
            else if (user.Budget < dto.Price * dto.Quantity)
            {
                return false;
            }
            return true;
        }
    }
}

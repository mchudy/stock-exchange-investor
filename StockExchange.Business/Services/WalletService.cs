using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Models;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Services
{
    public interface IWalletService
    {
        UserWalletDto GetUserWallet(int userId);
        bool CreateWallet(int userId, decimal budget);
    }

    public class WalletSerivce : IWalletService
    {
        private readonly IRepository<Wallet> _walletRepository;
        private readonly IRepository<User> _usersRepository;

        public WalletSerivce(IRepository<Wallet> walletRepository, IRepository<User> usersRepository)
        {
            _walletRepository = walletRepository;
            _usersRepository = usersRepository;
        }

        public UserWalletDto GetUserWallet(int userId)
        {
            var wallet = _walletRepository.GetQueryable().Where(w=>w.User.Id==userId).
                Include(w=>w.Transactions).FirstOrDefault();
            if (wallet == null) return null;
            return new UserWalletDto()
            {
                Budget = wallet.Budget,
                Transactions = wallet.Transactions.Select(t => new UserTransactionDto()
                {
                    Date = t.Date,
                    Price = t.Price,
                    Quantity = t.Quantity,
                    CompanyName = t.Company.Name
                }).ToList()
            };
        }

        public bool CreateWallet(int userId, decimal budget)
        {
            User user = _usersRepository.GetQueryable().FirstOrDefault(u => u.Id == userId);
            if (user == null) return false;
            Wallet wallet = new Wallet()
            {
                Budget = budget,
                User = user
            };
            _walletRepository.Insert(wallet);
            _walletRepository.Save();
            return true;
        }
    }
}

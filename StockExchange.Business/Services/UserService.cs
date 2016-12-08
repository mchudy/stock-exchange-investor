using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Linq;
using StockExchange.Business.ServiceInterfaces;

namespace StockExchange.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public void EditBudget(int userId, decimal newBudget)
        {
            var user = _userRepository.GetQueryable().FirstOrDefault(u => u.Id == userId);
            user.Budget = newBudget;
            _userRepository.Save();
        }
    }
}

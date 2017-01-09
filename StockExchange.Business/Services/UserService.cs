using StockExchange.Business.ErrorHandling;
using StockExchange.Business.Exceptions;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides methods for operating on users
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// Creates a new instance of <see cref="UserService"/>
        /// </summary>
        /// <param name="userRepository"></param>
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task EditBudget(int userId, decimal newBudget)
        {
            var user = await _userRepository.GetQueryable().FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
                user.Budget = newBudget;
            else
                throw new BusinessException(nameof(userId), "User does not exist", ErrorStatus.DataNotFound);
            await _userRepository.Save();
        }
    }
}

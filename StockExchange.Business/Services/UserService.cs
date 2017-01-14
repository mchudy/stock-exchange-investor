using StockExchange.Business.ErrorHandling;
using StockExchange.Business.Exceptions;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides methods for operating on users
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Creates a new instance of <see cref="UserService"/>
        /// </summary>
        /// <param name="userRepository"></param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task EditBudget(int userId, decimal newBudget)
        {
            var user = await _userRepository.GetUser(userId);
            if (user != null)
                user.Budget = newBudget;
            else
                throw new BusinessException(nameof(userId), "User does not exist", ErrorStatus.DataNotFound);
            await _userRepository.Save();
        }
    }
}

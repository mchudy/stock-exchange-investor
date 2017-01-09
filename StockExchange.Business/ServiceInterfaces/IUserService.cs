using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    /// <summary>
    /// Provides methods for operating on users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Edits the user's free budget
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="newBudget">The new budget</param>
        Task EditBudget(int userId, decimal newBudget);
    }
}
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface IUserService
    {
        Task EditBudget(int userId, decimal newBudget);
    }
}
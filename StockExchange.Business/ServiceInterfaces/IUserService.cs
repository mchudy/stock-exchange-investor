namespace StockExchange.Business.ServiceInterfaces
{
    public interface IUserService
    {
        void EditBudget(int userId, decimal newBudget);
    }
}
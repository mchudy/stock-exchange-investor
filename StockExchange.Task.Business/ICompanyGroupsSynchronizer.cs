namespace StockExchange.Task.Business
{
    /// <summary>
    /// Synchronizes company groups
    /// </summary>
    public interface ICompanyGroupsSynchronizer
    {
        /// <summary>
        /// Updates company groups in the database
        /// </summary>
        /// <returns></returns>
        System.Threading.Tasks.Task UpdateCompanyGroups();
    }
}
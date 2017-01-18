using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.IRepositories
{
    /// <summary>
    /// Repository for <see cref="Company"/> entities
    /// </summary>
    public interface ICompanyRepository : IRepository<Company>
    {
        /// <summary>
        /// Returns all companies
        /// </summary>
        /// <returns>List of all companies</returns>
        Task<IList<Company>> GetCompanies();

        /// <summary>
        /// Returns all company names
        /// </summary>
        /// <returns>List of all company names</returns>
        Task<IList<string>> GetCompanyNames();

        /// <summary>
        /// Returns companies by given ids
        /// </summary>
        /// <param name="ids">Ids of companies to find</param>
        /// <returns>List of companies with given ids</returns>
        Task<IList<Company>> GetCompanies(IList<int> ids);
    }
}
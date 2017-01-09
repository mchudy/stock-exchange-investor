using StockExchange.Business.Models.Company;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.Business.ServiceInterfaces
{
    /// <summary>
    /// Provides methods for interacting with <see cref="Company"/> entities
    /// </summary>
    public interface ICompanyService
    {
        /// <summary>
        /// Returns all companies' names
        /// </summary>
        /// <returns>The company names</returns>
        Task<IList<string>> GetCompanyNames();

        /// <summary>
        /// Returns all companies
        /// </summary>
        /// <returns>The list of companies</returns>
        Task<IList<CompanyDto>> GetCompanies();

        /// <summary>
        /// Returns companies by ids
        /// </summary>
        /// <param name="ids">Ids of the companies to return</param>
        /// <returns>THe list of companies</returns>
        Task<IList<CompanyDto>> GetCompanies(IList<int> ids);
    }
}

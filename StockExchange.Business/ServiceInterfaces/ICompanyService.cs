using System.Collections.Generic;
using System.Threading.Tasks;
using StockExchange.Business.Models.Company;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ICompanyService
    {
        Task<IList<string>> GetCompanyNames();

        Task<IList<CompanyDto>> GetCompanies();

        Task<IList<CompanyDto>> GetCompanies(IList<int> ids);
    }
}

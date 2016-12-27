using System.Collections.Generic;
using StockExchange.Business.Models.Company;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ICompanyService
    {
        IEnumerable<string> GetCompanyNames();

        IList<CompanyDto> GetAllCompanies();

        IList<CompanyDto> GetCompanies(IList<int> ids);
    }
}

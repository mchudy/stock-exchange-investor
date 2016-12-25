using StockExchange.Business.Models;
using System.Collections.Generic;

namespace StockExchange.Business.ServiceInterfaces
{
    public interface ICompanyService
    {
        IEnumerable<string> GetCompanyNames();

        IList<CompanyDto> GetAllCompanies();
    }
}

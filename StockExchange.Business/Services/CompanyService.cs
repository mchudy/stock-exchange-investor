using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Models.Company;

namespace StockExchange.Business.Services
{
    public sealed class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;

        public CompanyService(IRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public IEnumerable<string> GetCompanyNames()
        {
            return _companyRepository.GetQueryable().Select(item => item.Code).Distinct().ToList();
        }

        public IList<CompanyDto> GetAllCompanies()
        {
            return _companyRepository.GetQueryable().Select(c => new CompanyDto
            {
                Code = c.Code,
                Id = c.Id,
                Name = c.Code
            }).ToList();
        }
    }
}

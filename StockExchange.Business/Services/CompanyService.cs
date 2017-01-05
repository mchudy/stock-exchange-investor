using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IList<string>> GetCompanyNames()
        {
            return await _companyRepository.GetQueryable().Select(item => item.Code).Distinct().ToListAsync();
        }

        public async Task<IList<CompanyDto>> GetCompanies()
        {
            return await _companyRepository.GetQueryable().Select(c => new CompanyDto
            {
                Code = c.Code,
                Id = c.Id,
                Name = c.Code
            }).ToListAsync();
        }

        public async Task<IList<CompanyDto>> GetCompanies(IList<int> ids)
        {
            return await _companyRepository.GetQueryable().Where(item => ids.Contains(item.Id)).Select(item => new CompanyDto
            {
                Name = item.Code,
                Code = item.Code,
                Id = item.Id
            }).ToListAsync();
        }
    }
}

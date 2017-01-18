using StockExchange.Business.Models.Company;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides methods for interacting with <see cref="Company"/> entities
    /// </summary>
    public sealed class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        /// <summary>
        /// Creates a new instance of <see cref="CompanyService"/>
        /// </summary>
        /// <param name="companyRepository"></param>
        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        /// <inheritdoc />
        public async Task<IList<string>> GetCompanyNames()
        {
            return await _companyRepository.GetCompanyNames();
        }

        /// <inheritdoc />
        public async Task<IList<CompanyDto>> GetCompanies()
        {
            var companies = await _companyRepository.GetCompanies();
            return companies.Select(c => new CompanyDto
            {
                Code = c.Code,
                Id = c.Id,
                Name = c.Code
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<IList<CompanyDto>> GetCompanies(IList<int> ids)
        {
           var companies = await _companyRepository.GetCompanies(ids);
           return companies.Select(item => new CompanyDto
            {
                Name = item.Code,
                Code = item.Code,
                Id = item.Id
            }).ToList();
        }
    }
}

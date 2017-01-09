using StockExchange.Business.Models.Company;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.Business.Services
{
    /// <summary>
    /// Provides methods for interacting with <see cref="Company"/> entities
    /// </summary>
    public sealed class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;

        /// <summary>
        /// Creates a new instance of <see cref="CompanyService"/>
        /// </summary>
        /// <param name="companyRepository"></param>
        public CompanyService(IRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        /// <inheritdoc />
        public async Task<IList<string>> GetCompanyNames()
        {
            return await _companyRepository.GetQueryable().Select(item => item.Code).Distinct().ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IList<CompanyDto>> GetCompanies()
        {
            return await _companyRepository.GetQueryable().Select(c => new CompanyDto
            {
                Code = c.Code,
                Id = c.Id,
                Name = c.Code
            }).ToListAsync();
        }

        /// <inheritdoc />
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

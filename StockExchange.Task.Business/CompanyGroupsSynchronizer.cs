using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.Task.Business.Data;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Task.Business
{
    public class CompanyGroupsSynchronizer : ICompanyGroupsSynchronizer
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<CompanyGroup> _groupRepository;

        public CompanyGroupsSynchronizer(IRepository<Company> companyRepository, IRepository<CompanyGroup> groupRepository)
        {
            _companyRepository = companyRepository;
            _groupRepository = groupRepository;
        }

        public async System.Threading.Tasks.Task UpdateCompanyGroups()
        {
            foreach (var groupEntry in CompanyGroupsData.Groups)
            {
                string groupName = groupEntry.Key;
                List<string> companyNames = groupEntry.Value;
                                 
                var group = _groupRepository.GetQueryable().FirstOrDefault(g => g.Name == groupName);
                if (group == null)
                {
                    group = new CompanyGroup {Name = groupName};
                    var companies = _companyRepository.GetQueryable().Where(c => companyNames.Contains(c.Code));
                    var companyGroupCompanies = companyNames.Select(n => new CompanyGroupCompany
                    {
                        CompanyId = companies.FirstOrDefault(c => c.Code == n).Id,
                        CompanyGroup = group
                    });
                    foreach (var companyGroupCompany in companyGroupCompanies)
                    {
                        group.CompanyGroupCompanies.Add(companyGroupCompany);
                    }
                    _groupRepository.Insert(group);
                }
            }
            await _groupRepository.Save();
        }
    }
}

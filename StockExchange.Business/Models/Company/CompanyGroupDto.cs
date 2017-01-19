using System.Collections.Generic;

namespace StockExchange.Business.Models.Company
{
    public class CompanyGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> CompanyIds { get; set; } = new List<int>();
    }
}

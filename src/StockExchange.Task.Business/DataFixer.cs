using log4net;
using StockExchange.DataAccess.IRepositories;
using StockExchange.DataAccess.Models;
using StockExchange.Task.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace StockExchange.Task.Business
{
    /// <summary>
    /// Fixes data downloaded from the GPW sources
    /// </summary>
    public class DataFixer : IDataFixer
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IRepository<Price> _priceRepository;

        private readonly Dictionary<string, List<Price>> _correctionData = new Dictionary<string, List<Price>>
        {
            {
                "ASSECOPOL", new List<Price>
                {
                    new Price { Date = new DateTime(2007, 1, 3), ClosePrice = 55.30m },
                    new Price { Date = new DateTime(2007, 1, 4), ClosePrice = 55.00m },
                    new Price { Date = new DateTime(2007, 1, 5), ClosePrice = 56.00m },
                    new Price { Date = new DateTime(2007, 1, 8), ClosePrice = 56.10m },
                    new Price { Date = new DateTime(2007, 1, 9), ClosePrice = 60.00m },
                    new Price { Date = new DateTime(2007, 1, 10), ClosePrice = 59.25m },
                    new Price { Date = new DateTime(2007, 1, 11), ClosePrice = 60.00m },
                    new Price { Date = new DateTime(2007, 1, 12), ClosePrice = 63.30m },
                    new Price { Date = new DateTime(2007, 1, 15), ClosePrice = 68.30m }
                }
            },
            {
                "GETINOBLE", new List<Price>
                {
                    new Price { Date = new DateTime(2012, 06, 15), ClosePrice = 1.35m }
                }
            }
        };

        /// <summary>
        /// Creates a new instance of <see cref="DataFixer"/>
        /// </summary>
        /// <param name="priceRepository"></param>
        public DataFixer(IRepository<Price> priceRepository)
        {
            _priceRepository = priceRepository;
        }


        /// <inheritdoc />
        public async System.Threading.Tasks.Task FixData()
        {
            logger.Info("Beginning fixing data");

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await DeleteWrongEntries();
                await UpdateIncorrectData();
                scope.Complete();
            }

            logger.Info("Finished fixing data");
        }

        private async System.Threading.Tasks.Task DeleteWrongEntries()
        {
            // delete entries for which companies weren't actually listed on GPW on the given date 
            // (caused by incorrect data loaded from GPW site, spooq doesn't return those dates)
            var toDelete = _priceRepository.GetQueryable()
                .Where(PriceValidationHelper.IsInvalidExpression());
            logger.Info($"Found {toDelete.Count()} records to delete");
            if (toDelete.Any())
            {
                await _priceRepository.RemoveRange(toDelete);
                await _priceRepository.Save();
            }
            logger.Info("Deleted successfully\n");
        }

        private async System.Threading.Tasks.Task UpdateIncorrectData()
        {
            // some rare weird cases where close price is wrong, needs manual correction
            var toUpdate = _priceRepository.GetQueryable()
                .Include(p => p.Company)
                .Where(p => p.ClosePrice > p.HighPrice && p.ClosePrice > p.LowPrice && p.OpenPrice != 0)
                .ToList();

            logger.Info($"Found {toUpdate.Count} records with incorrect data");
            foreach (var companyPrices in toUpdate.GroupBy(p => p.CompanyId))
            {
                var company = companyPrices.FirstOrDefault()?.Company;
                if(company == null)
                    continue;

                CorrectPrices(company, companyPrices);
            }
            await _priceRepository.Save();
            logger.Info("Finished updating incorrect data");
        }

        private void CorrectPrices(Company company, IGrouping<int, Price> companyPrices)
        {
            List<Price> correctPrices;
            if (_correctionData.TryGetValue(company.Name, out correctPrices) ||
                _correctionData.TryGetValue(company.Code, out correctPrices))
            {
                foreach (var price in companyPrices)
                {
                    var correctPrice = correctPrices.FirstOrDefault(p => p.Date == price.Date);
                    if (correctPrice == null)
                    {
                        logger.Warn(
                            $"Incorrect data for company {company.Id} on day {price.Date.ToShortDateString()} " +
                            "but no data for correction");
                    }
                    else
                    {
                        price.ClosePrice = correctPrice.ClosePrice;
                    }
                }
            }
            else
            {
                logger.Warn(
                    $"Incorrect data for company {company.Id} records {string.Join(",", companyPrices.Select(p => p.Id))} " +
                    "but no data for correction");
            }
        }
    }
}

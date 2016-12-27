using StockExchange.Business.Models.Transaction;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Filters;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.Transactions;
using System;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class TransactionsController : BaseController
    {
        private readonly ITransactionsService _transactionsService;
        private readonly ICompanyService _companyService;

        public TransactionsController(ITransactionsService transactionsService, ICompanyService companyService)
        {
            _transactionsService = transactionsService;
            _companyService = companyService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new AddTransactionViewModel
            {
                Companies = _companyService.GetAllCompanies()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult TransactionsTable()
        {
            var transactions = _transactionsService.GetUserTransactions(CurrentUserId);
            return PartialView("_TransactionsTable", transactions);
        }

        [HttpPost]
        [HandleJsonError]
        public ActionResult AddTransaction(AddTransactionViewModel model)
        {
            if (!ModelState.IsValid)
                return JsonErrorResult(ModelState);

            var dto = BuildUserTransactionDto(model);
            _transactionsService.AddUserTransaction(dto);
            return new JsonNetResult(new { dto.Id } );
        }

        private UserTransactionDto BuildUserTransactionDto(AddTransactionViewModel model)
        {
            return new UserTransactionDto
            {
                Date = DateTime.Now,
                CompanyId = model.SelectedCompanyId,
                Price = model.Price,
                Quantity = model.TransactionType == TransactionActionType.Buy ? model.Quantity : -model.Quantity,
                UserId = CurrentUserId
            };
        }
    }
}
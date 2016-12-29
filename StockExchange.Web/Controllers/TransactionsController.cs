using StockExchange.Business.Models.Transaction;
using StockExchange.Business.ServiceInterfaces;
using StockExchange.Web.Filters;
using StockExchange.Web.Helpers.Json;
using StockExchange.Web.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using StockExchange.Business.Extensions;
using StockExchange.Business.Models.Filters;
using StockExchange.Business.Models.Price;
using StockExchange.Web.Helpers;
using StockExchange.Web.Models.DataTables;

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
            return View(new TransactionViewModel { AddTransactionViewModel = model, Transactions = new List<UserTransactionDto>() });
        }

        [HttpGet]
        public ActionResult GetTransactionsTable(DataTableMessage<TransactionFilter> dataTableMessage)
        {
            var searchMessage = DataTableMessageConverter.ToPagedFilterDefinition(dataTableMessage);
            var pagedList = _transactionsService.GetUserTransactions(CurrentUserId, searchMessage);
            var model = BuildDataTableResponse(dataTableMessage, pagedList);
            return new JsonNetResult(model, false);
        }

        [HttpPost]
        [HandleJsonError]
        public ActionResult AddTransaction(AddTransactionViewModel model)
        {
            if (!ModelState.IsValid)
                return JsonErrorResult(ModelState);

            var dto = BuildUserTransactionDto(model);
            _transactionsService.AddUserTransaction(dto);
            return new JsonNetResult(new { dto.Id });
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static DataTableResponse<UserTransactionDto> BuildDataTableResponse(DataTableMessage<TransactionFilter> dataTableMessage, PagedList<UserTransactionDto> pagedList)
        {
            var model = new DataTableResponse<UserTransactionDto>
            {
                RecordsFiltered = pagedList.TotalCount,
                RecordsTotal = pagedList.TotalCount,
                Data = pagedList,
                Draw = dataTableMessage.Draw
            };
            return model;
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
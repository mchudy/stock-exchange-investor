using StockExchange.Business.Services;
using StockExchange.Web.Models;
using System;
using System.Web.Mvc;

namespace StockExchange.Web.Controllers
{
    [Authorize]
    public class SimulationsController : Controller
    {
        private readonly IPriceService _priceService;

        public SimulationsController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        public ActionResult Index()
        {
            var model = GetViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult RunSimulation(SimulationViewModel model)
        {
            return RedirectToAction("Results");
        }

        [HttpGet]
        public ActionResult Results()
        {
            return View();
        }

        private SimulationViewModel GetViewModel()
        {
            var model = new SimulationViewModel
            {
                Companies = _priceService.GetAllCompanies(),
                StartDate = new DateTime(2006, 01, 01),
                EndDate = DateTime.Today
            };
            return model;
        }
    }
}
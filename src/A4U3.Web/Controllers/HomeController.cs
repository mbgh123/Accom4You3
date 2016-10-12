using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using A4U3.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using A4U3.Domain.Models;
using A4U3.Web.Infrastructure;

namespace A4U3.Web.Controllers
{
    [TypeFilter(typeof(MyAudit))]
    public class HomeController : Controller
    {
        private IOptions<ConfigOptions> _configOptions;
        private IRepository _repo;

        public HomeController(IOptions<ConfigOptions> configOptions, IRepository repository)
        {
            // MVC6 This is how to access the json config items
            _configOptions = configOptions;
            _repo = repository ;
        }

        public IActionResult Index()
        {
            // MVC6 This is how to access the json config items
            Debug.WriteLine(_configOptions.Value.ShowSearch);

            return View();
        }
        
        public IActionResult Contact()
        {

            //var test = _repo.StaticData.GetRobotList();

            //var test2 = _repo.GetPropertiesAndChildren();

            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {

            return View();
        }

        public IActionResult Landlords()
        {
            return View();
        }

        public IActionResult Tenants()
        {
            return View();
        }

        public IActionResult Sales()
        {
            return View();
        }

        public IActionResult PropertyBought()
        {
            return View();
        }

        public IActionResult FinancialServices()
        {
            return View();
        }

        public IActionResult PropertyMaintenance()
        {
            return View();
        }
    }
}

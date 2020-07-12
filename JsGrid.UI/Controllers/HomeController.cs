using System;
using System.Diagnostics;
using System.Text.Json;
using Bogus;
using Bogus.Extensions.UnitedStates;
using JsDataGrids.Service;
using JsDataGrids.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Bogus.Extensions;
using JsDataGrids.DataAccess.Models;
using JsDataGrids.UI.Extensions;
using JsDataGrids.UI.Faker;

namespace JsDataGrids.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }


        private void GenerateFakeDataAndSaveInSqlServer()
        {
            var data= GenerateFakeData.GenerateEmployeeData();
            //var distinctStates = data.DistinctByColumn(x => x.State).ToList();
            // var json = JsonSerializer.Serialize(employees);
             var result=DataService.BulkCopyToSqlServer(data);
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JsDataGrids.UI.Controllers
{
    public class JsGridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
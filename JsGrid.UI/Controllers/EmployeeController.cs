using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsDataGrids.DataAccess.Models;
using JsDataGrids.Service;
using JsDataGrids.UI.Extensions;
using JsDataGrids.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace JsDataGrids.UI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
    
        [HttpGet]
        [Route("api/employee/getFilterValues")]
        public async Task<IActionResult> GetFilterValues()
        {
            IQueryCollection httpQueryCollection = HttpContext.Request.Query;
            var param = httpQueryCollection["someParamName"];

            try
            {
                List<EmployeeGridFilterModel> dropDownFilterValues = await DataService.GetEmployeeGridFilterListAsync("1=1");

                EmployeeGridDropdownData data = new EmployeeGridDropdownData
                {
                    GenderList = dropDownFilterValues.OrderBy(x => x.Gender).DistinctByColumn(x => x.Gender).ToList(),
                    StateList = dropDownFilterValues.OrderBy(x => x.StateName).DistinctByColumn(x => x.StateName).ToList()
                };
                return Ok(new { data = data, success = true, message = "Successfully retrieve data." });
            }
            catch (Exception ex)
            {
                ////log error here...
                return Ok(new { data = string.Empty, success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/employee/getEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                IQueryCollection httpQueryCollection = HttpContext.Request.Query;
                var filters = httpQueryCollection.ToEmployeeGridFilters();
                Ref<int> totalRecordCount = new Ref<int>();

                List<Employee> data = await DataService.GetEmployeeGridDataAsync(filters, totalRecordCount);

                return Ok(new { items = data, totalCount=totalRecordCount.Value, success = true, message = "Successfully retrieve data." });
            }
            catch (Exception ex)
            {
                ////log error here...
                return Ok(new { items = string.Empty, success = false, message = ex.Message });
            }
        }
    }


}

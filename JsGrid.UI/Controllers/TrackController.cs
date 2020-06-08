using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using JsDataGrids.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace JsDataGrids.UI.Controllers
{
    [Route("api/track")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        public ActionResult Get()
        {
            var filters = GetFilters(HttpContext.Request.Query);
            
            int totalRecords = 0;
            var getTracks = DataService.GetTracks(filters.PageSize, filters.CurrentPage, filters.SortColumn,
                filters.SortOrder, filters.WhereCondititon, ref totalRecords);

            return Ok(new { items = getTracks, totalCount = totalRecords });
        }


        private GridFilters GetFilters(IQueryCollection filters)
        {
            var dict = new Dictionary<string, string>();
            foreach (var key in filters.Keys)
            {
                var @value = new StringValues();
                filters.TryGetValue(key, out @value);
                if (string.IsNullOrEmpty(@value)) continue;
                if (key.Contains("pageSize")) continue;
                if (key.Contains("pageIndex")) continue;
                if (key.Contains("sortField")) continue;
                if (key.Contains("sortOrder")) continue;
                dict.Add(key, @value.ToString());
            }
            var whereClause = "1=1 ";
            if (dict.Count > 0)
            {
                foreach (var kvp in dict)
                {
                    switch (kvp.Key)
                    {
                        case "id":
                            whereClause += "AND id='" + kvp.Value + "'";
                            break;
                        case "albumId":
                            whereClause += "AND albumId='" + kvp.Value + "'";
                            break;
                        case "trackName":
                            whereClause += "AND trackName like'%" + kvp.Value.Replace("'","") + "%'";
                            break;
                        case "composer":
                            whereClause += "AND composer like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "performer":
                            whereClause += "AND performer like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "featuring":
                            whereClause += "AND featuring like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "duration":
                            whereClause += "AND duration='" + kvp.Value + "'";
                            break;
                    }
                }
                whereClause = whereClause.Replace("1=1 AND", "");

            }

            return new GridFilters
            {
                PageSize = Convert.ToInt32(filters["pageSize"]),
                CurrentPage = Convert.ToInt32(filters["pageIndex"]),
                SortColumn = filters["sortField"],
                SortOrder = filters["sortOrder"],
                WhereCondititon=whereClause
            };

        }

        private class GridFilters
        {
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public string SortColumn { get; set; }
            public string SortOrder { get; set; }
            public string WhereCondititon { get; set; }
        }

    }
}
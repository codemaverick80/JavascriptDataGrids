using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsDataGrids.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace JsDataGrids.UI.Extensions
{
    public static class GridFilterExtensions
    {

        public static GridPagination ToEmployeeGridFilters(this IQueryCollection filters)
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
                        //// int values
                        //case "id":
                        //    whereClause += "AND Id=" + kvp.Value + "";
                        //    break;

                        case "id":
                            whereClause += "AND GuidId='" + kvp.Value + "'";
                            break;
                        case "salutation":
                            whereClause += "AND Salutation like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "firstName":
                            whereClause += "AND FirstName like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "middleName":
                            whereClause += "AND MiddleName like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "lastName":
                            whereClause += "AND LastName like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "dateOfBirth":
                            whereClause += "AND DateOfBirth like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "email":
                            whereClause += "AND Email like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "phone":
                            whereClause += "AND Phone like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "addressLine":
                            whereClause += "AND AddressLine like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "city":
                            whereClause += "AND City like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "zipCode":
                            whereClause += "AND ZipCode like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "salary":
                            whereClause += "AND Salary like'%" + kvp.Value.Replace("'", "") + "%'";
                            break;
                        case "sSN":
                            whereClause += "AND SSN ='" + kvp.Value + "'";
                            break;

                        //// Grid Dropdown Filters
                        case "state":
                            whereClause += kvp.Value == "0"
                                ? ""
                                : (kvp.Value.Trim() == "Null"
                                    ? "AND (state is null or state='') "
                                    : "AND state='" + kvp.Value + "'");
                            break;
                        case "gender":
                            whereClause += kvp.Value == "0"
                                ? ""
                                : (kvp.Value.Trim() == "Null"
                                    ? "AND (gender is null or gender='') "
                                    : "AND gender='" + kvp.Value + "'");
                            break;
                    }
                }
                whereClause = whereClause.Replace("1=1 AND", "");

            }

            return new GridPagination()
            {
                PageSize = Convert.ToInt32(filters["pageSize"]),
                CurrentPage = Convert.ToInt32(filters["pageIndex"]),
                SortColumn = filters["sortField"],
                SortOrder = filters["sortOrder"],
                WhereCondition = whereClause
            };

        }

    }
}

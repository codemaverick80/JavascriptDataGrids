using System.Collections.Generic;
using System.Threading.Tasks;
using JsDataGrids.DataAccess.Models;

namespace JsDataGrids.DataAccess
{
   public static class DataProvider
    {


        public static List<Track> GetTracks(int pageSize, int currentPage, string sortColumn, string sortOrder, string whereCondition,ref int totalCount)
        {
            return DataController.GetTracks(pageSize, currentPage, sortColumn,  sortOrder, whereCondition, ref totalCount);
        }

        public static List<Track> GetTracksUsingRawSQL()
        {
            return DataController.GetTracksUsingRawSQL();
        }
       
        public static List<Artist> GetArtists()
        {
            return DataController.GetArtists();
        }


       // GetEmployeeGridFilterListAsync
       public static async Task<List<EmployeeGridFilterModel>> GetEmployeeGridFilterListAsync(string where)
       {
           return await DataController.GetEmployeeGridFilterListAsync(where);
       }

       public static async Task<List<Employee>> GetEmployeeGridDataAsync(GridPagination pagination, Ref<int> totalRecords)
       {
           return await DataController.GetEmployeeGridDataAsync(pagination, totalRecords);
       }

        public static bool BulkCopyToSQLServer(List<Employee> employees)
        {
            return DataController.BulkCopyToSQLServer(employees);
        }


    }
}

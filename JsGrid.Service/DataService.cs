using System.Collections.Generic;
using System.Threading.Tasks;
using JsDataGrids.DataAccess;
using JsDataGrids.DataAccess.Models;

namespace JsDataGrids.Service
{
   public class DataService
    {
        public static List<Track> GetTracks(int pageSize, int currentPage, string sortColumn, string sortOrder, string whereCondition, ref int totalCount)
        {
            return DataProvider.GetTracks( pageSize,  currentPage,  sortColumn,  sortOrder, whereCondition, ref totalCount);
        }

        public static List<Track> GetTracksUsingRawSQL()
        {
            return DataProvider.GetTracksUsingRawSQL();
        }

        public static List<Artist> GetArtist()
        {
            return DataProvider.GetArtists();
        }

        public static async Task<List<EmployeeGridFilterModel>> GetEmployeeGridFilterListAsync(string where)
        {
            return await DataProvider.GetEmployeeGridFilterListAsync(where);
        }

        public static async Task<List<Employee>> GetEmployeeGridDataAsync(GridPagination pagination, Ref<int> totalRecords)
        {
            return await DataProvider.GetEmployeeGridDataAsync(pagination, totalRecords);
        }

        public static bool BulkCopyToSqlServer(List<Employee> employees)
        {
            return DataProvider.BulkCopyToSQLServer(employees);
        }

    }
}

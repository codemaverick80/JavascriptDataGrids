using System.Collections.Generic;
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

        public static bool BulkCopyToSQLServer(List<Employee> employees)
        {
            return DataController.BulkCopyToSQLServer(employees);
        }
    }
}

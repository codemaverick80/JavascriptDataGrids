using System.Collections.Generic;
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
    }
}

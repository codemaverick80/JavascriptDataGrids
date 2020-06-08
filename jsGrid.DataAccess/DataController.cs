using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JsDataGrids.DataAccess.Models;

namespace JsDataGrids.DataAccess
{
    class DataController
    {

        internal static List<Track> GetTracks(int pageSize, int currentPage,string sortColumn,string sortOrder, string whereCondition, ref int totalCount)
        {
            List<Track> tracks=new List<Track>();

            using (SqlConnection conn =
                new SqlConnection(
                    EnvironmentManager.GetConnectionString(EnvironmentManager.Database.DatabaseConnection)))
            {
                conn.Open();
                
                //using (SqlCommand command = new SqlCommand("dbo.uspGetAllTracks_New", conn))
                using (SqlCommand command = new SqlCommand("dbo.uspGetAllTracksWithCTE_NEW", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SortCol", sortColumn);
                    command.Parameters.AddWithValue("@SortOrder", sortOrder);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@CurrentPage", currentPage);
                    command.Parameters.AddWithValue("@WhereCondition", whereCondition);

                    command.Parameters.Add("@TotalRecords", SqlDbType.Int); //OUTPUT parameter
                    command.Parameters["@TotalRecords"].Direction = ParameterDirection.Output;
                    
                    
                    tracks = EntityMapper.CreateTrackList(command.ExecuteReader());
                    totalCount =Convert.ToInt32(command.Parameters["@TotalRecords"].Value);
                }
            }

            return tracks;
        }

        internal static List<Track> GetTracksUsingRawSQL()
        {
            List<Track> tracks = new List<Track>();

            using (SqlConnection conn =
                new SqlConnection(
                    EnvironmentManager.GetConnectionString(EnvironmentManager.Database.DatabaseConnection)))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand("SELECT top 100 * FROM [dbo].[Track]", conn))
                {
                    command.CommandType = CommandType.Text;
                    tracks = EntityMapper.CreateTrackList(command.ExecuteReader());
                }
            }

            return tracks;
        }

        internal static List<Artist> GetArtists()
        {
            List<Artist> artists = new List<Artist>();

            using (SqlConnection conn =
                new SqlConnection(
                    EnvironmentManager.GetConnectionString(EnvironmentManager.Database.DatabaseConnection)))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand("dbo.uspGetTracks", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    artists = EntityMapper.CreateArtistList(command.ExecuteReader());
                }
            }

            return artists;
        }


    }
}

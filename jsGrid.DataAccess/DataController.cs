using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JsDataGrids.DataAccess.Extensions;
using JsDataGrids.DataAccess.Models;

namespace JsDataGrids.DataAccess
{
    class DataController
    {


        internal static bool BulkCopyToSQLServer(List<Employee> employees)
        {
            try
            {
                var datatable = employees.ConvertToDataTable();
                using (SqlBulkCopy bulkCopy =
                    new SqlBulkCopy(
                        EnvironmentManager.GetConnectionString(EnvironmentManager.Database.DatabaseConnection)))
                {

                    bulkCopy.DestinationTableName = "Employee";
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.BulkCopyTimeout = 60 * 10; //10 minutes

                    bulkCopy.ColumnMappings.Clear();
                    bulkCopy.ColumnMappings.Add("Id", "GuidId");
                    bulkCopy.ColumnMappings.Add("Salutation", "Salutation");
                    bulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                    bulkCopy.ColumnMappings.Add("LastName", "LastName");
                    bulkCopy.ColumnMappings.Add("MiddleName", "MI");
                    bulkCopy.ColumnMappings.Add("Gender", "Gender");
                    bulkCopy.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
                    bulkCopy.ColumnMappings.Add("Email", "Email");
                    bulkCopy.ColumnMappings.Add("Phone", "Phone");
                    bulkCopy.ColumnMappings.Add("AddressLine", "AddressLine");
                    bulkCopy.ColumnMappings.Add("City", "City");
                    bulkCopy.ColumnMappings.Add("State", "State");
                    bulkCopy.ColumnMappings.Add("ZipCode", "ZipCode");
                    bulkCopy.ColumnMappings.Add("Salary", "Salary");
                    bulkCopy.ColumnMappings.Add("SSN", "SSN");
                    bulkCopy.WriteToServer(datatable);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }



        internal static List<Track> GetTracks(int pageSize, int currentPage, string sortColumn, string sortOrder, string whereCondition, ref int totalCount)
        {
            List<Track> tracks = new List<Track>();

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
                    totalCount = Convert.ToInt32(command.Parameters["@TotalRecords"].Value);
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

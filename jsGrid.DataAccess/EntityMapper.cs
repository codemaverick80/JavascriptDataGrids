using System;
using System.Collections.Generic;
using System.Data;
using JsDataGrids.DataAccess.Models;

namespace JsDataGrids.DataAccess
{
     class EntityMapper
    {

        internal static List<Track> CreateTrackList(IDataReader data)
        {
            var tracks=new List<Track>();

            using (data)
            {
                while (data.Read())
                {
                    var track = new Track
                    {
                        Id = System.Web.HttpUtility.HtmlEncode(data["Id"].ToString() ?? string.Empty),
                        TrackName = System.Web.HttpUtility.HtmlEncode(data["TrackName"].ToString() ?? string.Empty),
                        AlbumId = System.Web.HttpUtility.HtmlEncode(data["AlbumId"].ToString() ?? string.Empty),
                        Composer = System.Web.HttpUtility.HtmlEncode(data["Composer"].ToString() ?? string.Empty),
                        Performer = System.Web.HttpUtility.HtmlEncode(data["Performer"].ToString() ?? string.Empty),
                        Featuring = System.Web.HttpUtility.HtmlEncode(data["Featuring"].ToString() ?? string.Empty),
                        Duration = System.Web.HttpUtility.HtmlEncode(data["Duration"].ToString() ?? string.Empty)
                    };
                    tracks.Add(track);
                }
            }
            return tracks;
        }

        internal static List<Artist> CreateArtistList(IDataReader data)
        {
            var artists = new List<Artist>();

            using (data)
            {
                while (data.Read())
                {
                    var artist = new Artist
                    {
                        Id = System.Web.HttpUtility.HtmlEncode(data["Id"].ToString() ?? string.Empty),
                        ArtistName = System.Web.HttpUtility.HtmlEncode(data["Artist"].ToString() ?? string.Empty),
                        Tag = System.Web.HttpUtility.HtmlEncode(data["Tag"].ToString() ?? string.Empty),
                        YearsActive = System.Web.HttpUtility.HtmlEncode(data["YearsActive"].ToString() ?? string.Empty),
                        Biography = System.Web.HttpUtility.HtmlEncode(data["Biography"].ToString() ?? string.Empty)
                  
                    };
                    artists.Add(artist);
                }
            }
            return artists;
        }


        internal static List<EmployeeGridFilterModel> CreateEmployeeGridFilterList(IDataReader data)
        {
            var filterList = new List<EmployeeGridFilterModel>();

            using (data)
            {
                while (data.Read())
                {
                    var artist = new EmployeeGridFilterModel()
                    {
                        Gender = data["Gender"].ToString()?.Trim()== "" ? "Null" : data["Gender"].ToString(),
                        StateName = data["State"].ToString()?.Trim() == "" ? "Null" : data["State"].ToString()
                    };
                    filterList.Add(artist);
                }
            }
            return filterList;
        }


        internal static List<Employee> CreateEmployeeGridData(IDataReader data)
        {
            var gridData = new List<Employee>();

            using (data)
            {
                while (data.Read())
                {
                    var artist = new Employee()
                    {
                        Id =Guid.Parse(data["GuidId"].ToString() ?? string.Empty),
                        Salutation = data["Salutation"].ToString()??string.Empty,
                        FirstName = data["FirstName"].ToString() ?? string.Empty,
                        MiddleName = data["Mi"].ToString() ?? string.Empty,
                        LastName = data["LastName"].ToString() ?? string.Empty,
                        Gender = data["Gender"].ToString() ?? string.Empty,
                        DateOfBirth =data["DateOfBirth"].ToString() ?? string.Empty,
                        Email = data["Email"].ToString() ?? string.Empty,
                        Phone = data["Phone"].ToString() ?? string.Empty,
                        AddressLine = data["AddressLine"].ToString() ?? string.Empty,
                        City = data["City"].ToString() ?? string.Empty,
                        ZipCode = data["ZipCode"].ToString() ?? string.Empty,
                        State = data["State"].ToString() ?? string.Empty,
                        Salary =Convert.ToDecimal(data["Salary"].ToString() ?? "0.00"),
                        SSN = data["SSN"].ToString() ?? string.Empty
                    };
                    gridData.Add(artist);
                }
            }
            return gridData;
        }

    }
}

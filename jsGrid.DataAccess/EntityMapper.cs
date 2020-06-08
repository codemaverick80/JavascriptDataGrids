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

    }
}

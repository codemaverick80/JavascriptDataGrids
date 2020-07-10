using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using JsDataGrids.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace JsDataGrids.UI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {

        [HttpGet]
        [Route("api/download/excel")]
        public IActionResult DownloadExcelFile()     
        {
            var filters = GetFilters(HttpContext.Request.Query);
            int totalRecords = 0;
            var getTracks = DataService.GetTracks(1000, 1, null, null, "1=1", ref totalRecords);


            MemoryStream ms = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();


                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("Id", CellValues.String),
                    ConstructCell("AlbumId", CellValues.String),
                    ConstructCell("Track Name", CellValues.String),
                    ConstructCell("Performer", CellValues.String));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                // Inserting each Tracks
                foreach (var track in getTracks)
                {
                    row = new Row();
                    row.Append(
                        ConstructCell(track.Id, CellValues.String),
                        ConstructCell(track.AlbumId, CellValues.String),
                        ConstructCell(track.TrackName, CellValues.String),
                        ConstructCell(track.Performer, CellValues.String));

                    sheetData.AppendChild(row);
                }

                worksheetPart.Worksheet.Save();
            }

            string filename = "Tracks.xlsx";
            ms.Seek(0, SeekOrigin.Begin);
            //return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",filename);
            return File(ms, GetMimeTypesA()[".xlsx"], filename);
        }


        private Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }


        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt","text/plain"},
                {".pdf","application/pdf"},
                {".doc","application/vnd.ms-word"},
                {".docx","application/vnd.ms-word"},
                {".xls","application/vnd.ms-excel"},
                {".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png","image/png"},
                {".jpg","image/jpeg"},
                {".jpeg","image/jpeg"},
                {".gif","image/gif"},
                {".csv","text/csv"}
            };
        }


        private Dictionary<string, string> GetMimeTypesA()
        {
            return new Dictionary<string, string>
            {
                [".txt"] = "text/plain",
                [".pdf"] = "application/pdf",
                [".doc"] = "application/vnd.ms-word",
                [".docx"] = "application/vnd.ms-word",
                [".xls"] = "application/vnd.ms-excel",
                [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                [".png"] = "image/png",
                [".jpg"] = "image/jpeg",
                [".jpeg"] = "image/jpeg",
                [".gif"] = "image/gif",
                [".csv"] = "text/csv"
            };
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
                            whereClause += kvp.Value == "0" ? "" : "AND albumId='" + kvp.Value + "'";
                            break;
                        case "trackName":
                            whereClause += "AND trackName like'%" + kvp.Value.Replace("'", "") + "%'";
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
                WhereCondititon = whereClause
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

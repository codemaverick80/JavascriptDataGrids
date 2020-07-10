using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using JsDataGrids.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using JsDataGrids.DataAccess.Models;

namespace JsDataGrids.UI.Controllers
{
   // [Route("api/track")]
    [ApiController]
    public class TrackController : ControllerBase
    {

        [HttpGet]
        [Route("api/track")]
        public ActionResult Get()
        {
            var filters = GetFilters(HttpContext.Request.Query);
            //// NameValueCollection f = HttpUtility.ParseQueryString(filters);
            int totalRecords = 0;
            var getTracks = DataService.GetTracks(filters.PageSize, filters.CurrentPage, filters.SortColumn,
                filters.SortOrder, filters.WhereCondititon, ref totalRecords);

            return Ok(new { items = getTracks, totalCount = totalRecords });
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
                            whereClause += kvp.Value=="0" ? "": "AND albumId='" + kvp.Value + "'";
                            break;
                        case "trackName":
                            whereClause += "AND trackName like'%" + kvp.Value.Replace("'","") + "%'";
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
                WhereCondititon=whereClause
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













        
        private void CreateExcelFile(List<Track> data, string OutPutFileDirectory)
        {
            var datetime = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_");

            string fileFullname = Path.Combine(OutPutFileDirectory, "Output.xlsx");

            if (System.IO.File.Exists(fileFullname))
            {
                fileFullname = Path.Combine(OutPutFileDirectory, "Output_" + datetime + ".xlsx");
            }

            //using (SpreadsheetDocument package = SpreadsheetDocument.Create(fileFullname, SpreadsheetDocumentType.Workbook))
            //{
            //    CreatePartsForExcel(package, data);
            //}

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create("c:\\Users\\Banku\\Desktop\\mytest.xlsx", SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                // Append a new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Test"
                };

                Row row = new Row() { RowIndex = 1 };
                Cell header1 = new Cell() { CellReference = "A1", CellValue = new CellValue("Interval Period Timestamp"), DataType = CellValues.String };
                row.Append(header1);
                Cell header2 = new Cell() { CellReference = "B1", CellValue = new CellValue("Settlement Interval"), DataType = CellValues.String };
                row.Append(header2);
                Cell header3 = new Cell() { CellReference = "C1", CellValue = new CellValue("Aggregated Consumption Factor"), DataType = CellValues.String };
                row.Append(header3);
                Cell header4 = new Cell() { CellReference = "D1", CellValue = new CellValue("Loss Adjusted Aggregated Consumption"), DataType = CellValues.String };
                row.Append(header4);

                sheetData.Append(row);

                sheets.Append(sheet);

                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();
               

            }






        }

        private void CreatePartsForExcel(SpreadsheetDocument document, List<Track> data)
        {
            SheetData partSheetData = GenerateSheetdataForDetails(data);

            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);

           // WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            //GenerateWorkbookStylesPartContent(workbookStylesPart1);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData);
        }


        private void GenerateWorkbookPartContent(WorkbookPart workbookPart1)
        {
            Workbook workbook1 = new Workbook();
            Sheets sheets1 = new Sheets();
            Sheet sheet1 = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };
            sheets1.Append(sheet1);
            workbook1.Append(sheets1);
            workbookPart1.Workbook = workbook1;
        }

        private void GenerateWorksheetPartContent(WorksheetPart worksheetPart1, SheetData sheetData1)
        {
            Worksheet worksheet1 = new Worksheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            SheetDimension sheetDimension1 = new SheetDimension() { Reference = "A1" };

            SheetViews sheetViews1 = new SheetViews();

            SheetView sheetView1 = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
            Selection selection1 = new Selection() { ActiveCell = "A1", SequenceOfReferences = new ListValue<StringValue>() { InnerText = "A1" } };

            sheetView1.Append(selection1);

            sheetViews1.Append(sheetView1);
            SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.25D };

            PageMargins pageMargins1 = new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };
            worksheet1.Append(sheetDimension1);
            worksheet1.Append(sheetViews1);
            worksheet1.Append(sheetFormatProperties1);
            worksheet1.Append(sheetData1);
            worksheet1.Append(pageMargins1);
            worksheetPart1.Worksheet = worksheet1;
        }



        //private void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
        //{
        //    Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
        //    stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        //    stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

        //    Fonts fonts1 = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

        //    Font font1 = new Font();
        //    FontSize fontSize1 = new FontSize() { Val = 11D };
        //    Color color1 = new Color() { Theme = (UInt32Value)1U };
        //    FontName fontName1 = new FontName() { Val = "Calibri" };
        //    FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
        //    FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

        //    font1.Append(fontSize1);
        //    font1.Append(color1);
        //    font1.Append(fontName1);
        //    font1.Append(fontFamilyNumbering1);
        //    font1.Append(fontScheme1);

        //    Font font2 = new Font();
        //    Bold bold1 = new Bold();
        //    FontSize fontSize2 = new FontSize() { Val = 11D };
        //    Color color2 = new Color() { Theme = (UInt32Value)1U };
        //    FontName fontName2 = new FontName() { Val = "Calibri" };
        //    FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
        //    FontScheme fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };

        //    font2.Append(bold1);
        //    font2.Append(fontSize2);
        //    font2.Append(color2);
        //    font2.Append(fontName2);
        //    font2.Append(fontFamilyNumbering2);
        //    font2.Append(fontScheme2);

        //    fonts1.Append(font1);
        //    fonts1.Append(font2);

        //    Fills fills1 = new Fills() { Count = (UInt32Value)2U };

        //    Fill fill1 = new Fill();
        //    PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

        //    fill1.Append(patternFill1);

        //    Fill fill2 = new Fill();
        //    PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

        //    fill2.Append(patternFill2);

        //    fills1.Append(fill1);
        //    fills1.Append(fill2);

        //    Borders borders1 = new Borders() { Count = (UInt32Value)2U };

        //    Border border1 = new Border();
        //    LeftBorder leftBorder1 = new LeftBorder();
        //    RightBorder rightBorder1 = new RightBorder();
        //    TopBorder topBorder1 = new TopBorder();
        //    BottomBorder bottomBorder1 = new BottomBorder();
        //    DiagonalBorder diagonalBorder1 = new DiagonalBorder();

        //    border1.Append(leftBorder1);
        //    border1.Append(rightBorder1);
        //    border1.Append(topBorder1);
        //    border1.Append(bottomBorder1);
        //    border1.Append(diagonalBorder1);

        //    Border border2 = new Border();

        //    LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
        //    Color color3 = new Color() { Indexed = (UInt32Value)64U };

        //    leftBorder2.Append(color3);

        //    RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
        //    Color color4 = new Color() { Indexed = (UInt32Value)64U };

        //    rightBorder2.Append(color4);

        //    TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
        //    Color color5 = new Color() { Indexed = (UInt32Value)64U };

        //    topBorder2.Append(color5);

        //    BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
        //    Color color6 = new Color() { Indexed = (UInt32Value)64U };

        //    bottomBorder2.Append(color6);
        //    DiagonalBorder diagonalBorder2 = new DiagonalBorder();

        //    border2.Append(leftBorder2);
        //    border2.Append(rightBorder2);
        //    border2.Append(topBorder2);
        //    border2.Append(bottomBorder2);
        //    border2.Append(diagonalBorder2);

        //    borders1.Append(border1);
        //    borders1.Append(border2);

        //    CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
        //    CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

        //    cellStyleFormats1.Append(cellFormat1);

        //    CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)3U };
        //    CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
        //    CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyBorder = true };
        //    CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyFont = true, ApplyBorder = true };

        //    cellFormats1.Append(cellFormat2);
        //    cellFormats1.Append(cellFormat3);
        //    cellFormats1.Append(cellFormat4);

        //    CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
        //    CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

        //    cellStyles1.Append(cellStyle1);
        //    DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
        //    TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

        //    StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

        //    StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
        //    stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
        //    X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

        //    stylesheetExtension1.Append(slicerStyles1);

        //    StylesheetExtension stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
        //    stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
        //    X15.TimelineStyles timelineStyles1 = new X15.TimelineStyles() { DefaultTimelineStyle = "TimeSlicerStyleLight1" };

        //    stylesheetExtension2.Append(timelineStyles1);

        //    stylesheetExtensionList1.Append(stylesheetExtension1);
        //    stylesheetExtensionList1.Append(stylesheetExtension2);

        //    stylesheet1.Append(fonts1);
        //    stylesheet1.Append(fills1);
        //    stylesheet1.Append(borders1);
        //    stylesheet1.Append(cellStyleFormats1);
        //    stylesheet1.Append(cellFormats1);
        //    stylesheet1.Append(cellStyles1);
        //    stylesheet1.Append(differentialFormats1);
        //    stylesheet1.Append(tableStyles1);
        //    stylesheet1.Append(stylesheetExtensionList1);

        //    workbookStylesPart1.Stylesheet = stylesheet1;
        //}

        //private void GenerateWorkbookPartContent(WorkbookPart workbookPart1)
        //{
        //    Workbook workbook1 = new Workbook();
        //    Sheets sheets1 = new Sheets();
        //    Sheet sheet1 = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };
        //    sheets1.Append(sheet1);
        //    workbook1.Append(sheets1);
        //    workbookPart1.Workbook = workbook1;
        //}

        private SheetData GenerateSheetdataForDetails(List<Track> data)
        {
            SheetData sheetData1 = new SheetData();
            sheetData1.Append(CreateHeaderRowForExcel());

            foreach (var testmodel in data)
            {
                Row partsRows = GenerateRowForChildPartDetail(testmodel);
                sheetData1.Append(partsRows);
            }
            return sheetData1;
        }

        private Row CreateHeaderRowForExcel()
        {
            Row workRow = new Row();
            workRow.Append(CreateCell("Id", 2U));
            workRow.Append(CreateCell("TrackName", 2U));
            workRow.Append(CreateCell("AlbumId", 2U));
            workRow.Append(CreateCell("Performer", 2U));
            return workRow;
        }

        private Row GenerateRowForChildPartDetail(Track testmodel)
        {
            Row tRow = new Row();
            tRow.Append(CreateCell(testmodel.Id.ToString()));
            tRow.Append(CreateCell(testmodel.TrackName.ToString()));
            tRow.Append(CreateCell(testmodel.AlbumId.ToString()));
            tRow.Append(CreateCell(testmodel.Performer.ToString()));

            return tRow;
        }

        private Cell CreateCell(string text)
        {
            Cell cell = new Cell();
            cell.StyleIndex = 1U;
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new CellValue(text);
            return cell;
        }

        private Cell CreateCell(string text, uint styleIndex)
        {
            Cell cell = new Cell();
            cell.StyleIndex = styleIndex;
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new CellValue(text);
            return cell;
        }

        private EnumValue<CellValues> ResolveCellDataTypeOnValue(string text)
        {
            int intVal;
            double doubleVal;
            if (int.TryParse(text, out intVal) || double.TryParse(text, out doubleVal))
            {
                return CellValues.Number;
            }
            else
            {
                return CellValues.String;
            }
        }



    }
}
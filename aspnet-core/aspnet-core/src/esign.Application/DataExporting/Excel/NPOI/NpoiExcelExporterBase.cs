using System;
using System.Collections.Generic;
using System.IO;
using Abp.AspNetZeroCore.Net;
using Abp.Collections.Extensions;
using Abp.Dependency;
using esign.Dto;
using esign.Storage;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using System.Drawing;
using GemBox.Spreadsheet;

namespace esign.DataExporting.Excel.NPOI
{
    public abstract class NpoiExcelExporterBase : esignServiceBase, ITransientDependency
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private IWorkbook _workbook;

        private readonly Dictionary<string, ICellStyle> _dateCellStyles = new();
        private readonly Dictionary<string, IDataFormat> _dateDateDataFormats = new();

        private ICellStyle GetDateCellStyle(ICell cell, string dateFormat)
        {
            if (_workbook != cell.Sheet.Workbook)
            {
                _dateCellStyles.Clear();
                _dateDateDataFormats.Clear();
                _workbook = cell.Sheet.Workbook;
            }

            if (_dateCellStyles.ContainsKey(dateFormat))
            {
                return _dateCellStyles.GetValueOrDefault(dateFormat);
            }

            var cellStyle = cell.Sheet.Workbook.CreateCellStyle();
            _dateCellStyles.Add(dateFormat, cellStyle);
            return cellStyle;
        }

        private IDataFormat GetDateDataFormat(ICell cell, string dateFormat)
        {
            if (_workbook != cell.Sheet.Workbook)
            {
                _dateDateDataFormats.Clear();
                _workbook = cell.Sheet.Workbook;
            }

            if (_dateDateDataFormats.ContainsKey(dateFormat))
            {
                return _dateDateDataFormats.GetValueOrDefault(dateFormat);
            }

            var dataFormat = cell.Sheet.Workbook.CreateDataFormat();
            _dateDateDataFormats.Add(dateFormat, dataFormat);
            return dataFormat;
        }

        protected NpoiExcelExporterBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        protected FileDto CreateExcelPackage(string fileName, Action<XSSFWorkbook> creator)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var workbook = new XSSFWorkbook();

            creator(workbook);

            Save(workbook, file);

            return file;
        }

        protected void AddHeader(ISheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            sheet.CreateRow(0);

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i, headerTexts[i]);
            }
        }

        protected void AddHeader(ISheet sheet, int columnIndex, string headerText)
        {
            var cell = sheet.GetRow(0).CreateCell(columnIndex);
            cell.SetCellValue(headerText);
            var cellStyle = sheet.Workbook.CreateCellStyle();
            var font = sheet.Workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 12;
            cellStyle.SetFont(font);
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.FillForegroundColor = IndexedColors.SkyBlue.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cell.CellStyle = cellStyle;
        }

        protected void AddObjects<T>(ISheet sheet, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }
           
            ICellStyle cellStyleDate = sheet.Workbook.CreateCellStyle();
            cellStyleDate.BorderLeft = BorderStyle.Thin;
            cellStyleDate.BorderRight = BorderStyle.Thin;
            cellStyleDate.BorderBottom = BorderStyle.Thin;
            cellStyleDate.BorderTop = BorderStyle.Thin;

            ICellStyle cellStyleDateTime = sheet.Workbook.CreateCellStyle();
            cellStyleDateTime.BorderLeft = BorderStyle.Thin;
            cellStyleDateTime.BorderRight = BorderStyle.Thin;
            cellStyleDateTime.BorderBottom = BorderStyle.Thin;
            cellStyleDateTime.BorderTop = BorderStyle.Thin;

            ICellStyle cellStyleInt = sheet.Workbook.CreateCellStyle();
            cellStyleInt.BorderLeft = BorderStyle.Thin;
            cellStyleInt.BorderRight = BorderStyle.Thin;
            cellStyleInt.BorderBottom = BorderStyle.Thin;
            cellStyleInt.BorderTop = BorderStyle.Thin;

            ICellStyle cellStyleDecimal = sheet.Workbook.CreateCellStyle();
            cellStyleDecimal.BorderLeft = BorderStyle.Thin;
            cellStyleDecimal.BorderRight = BorderStyle.Thin;
            cellStyleDecimal.BorderBottom = BorderStyle.Thin;
            cellStyleDecimal.BorderTop = BorderStyle.Thin;

            ICellStyle cellStyleBorder = sheet.Workbook.CreateCellStyle();
            cellStyleBorder.BorderLeft = BorderStyle.Thin;
            cellStyleBorder.BorderRight = BorderStyle.Thin;
            cellStyleBorder.BorderBottom = BorderStyle.Thin;
            cellStyleBorder.BorderTop = BorderStyle.Thin;

            for (var i = 1; i <= items.Count; i++)
            {
                var row = sheet.CreateRow(i);

                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    var cell = row.CreateCell(j);
                    cell.CellStyle = cellStyleBorder;
                    var value = propertySelectors[j](items[i - 1]);
                    if (value != null)
                    {
                        if (value.GetType().Name.ToUpper() == "DATETIME")
                        {
                            try
                            {
                                cell.SetCellValue(DateTime.Parse(value.ToString()));
                                if (DateTime.Parse(value.ToString()).TimeOfDay.ToString() == "00:00:00")
                                {
                                    cellStyleDate.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("dd/MM/yyyy");
                                    cell.CellStyle = cellStyleDate;
                                }
                                else
                                {
                                    cellStyleDateTime.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("dd/MM/yyyy HH:mm:ss");
                                    cell.CellStyle = cellStyleDateTime;
                                }
                            }
                            catch
                            {
                                cell.SetCellValue(value.ToString());
                            }
                        }
                        else if(value.GetType().Name.ToUpper().Substring(0,3) == "INT")
                        {
                            if (value.GetType().Name.Length>=5 && value.GetType().Name.ToUpper()=="INT64")
                            {
                                try
                                {
                                    cell.SetCellValue(int.Parse(value.ToString()));
                                }
                                catch
                                {
                                    cell.SetCellValue(value.ToString());
                                }
                            } 
                            else
                            {   
                                try
                                {
                                    cell.SetCellValue(int.Parse(value.ToString()));
                                    cellStyleInt.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0");
                                    cell.CellStyle = cellStyleInt;
                                }
                                catch
                                {
                                    cell.SetCellValue(value.ToString());
                                }
                            }
                        }
                        else if (value.GetType().Name.ToUpper().Substring(0, 3) == "DEC")
                        {
                            try
                            {
                                cell.SetCellValue(double.Parse(value.ToString()));
                                cellStyleDecimal.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat("#,##0.00");
                                cell.CellStyle = cellStyleDecimal;
                            }
                            catch
                            {
                                cell.SetCellValue(value.ToString());
                            }
                        }
                        else
                        {
                            cell.SetCellValue(value.ToString());
                        }
                    }
                }
            }

            for (var i = 0; i < sheet.GetRow(0).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }

        protected virtual void Save(XSSFWorkbook excelPackage, FileDto file)
        {
            using (var stream = new MemoryStream())
            {
                excelPackage.Write(stream);
                _tempFileCacheManager.SetFile(file.FileToken, stream.ToArray());
            }
        }

        protected void SetCellDataFormat(ICell cell, string dataFormat)
        {
            if (cell == null)
                return;

            var dateStyle = GetDateCellStyle(cell, dataFormat);
            var format = GetDateDataFormat(cell, dataFormat);

            dateStyle.DataFormat = format.GetFormat(dataFormat);
            cell.CellStyle = dateStyle;
            if (DateTime.TryParse(cell.StringCellValue, out var datetime))
                cell.SetCellValue(datetime);
        }
    }
}

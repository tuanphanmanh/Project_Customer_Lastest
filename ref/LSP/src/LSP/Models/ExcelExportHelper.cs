using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using System.Web.UI.WebControls;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;


namespace LSP.Models
{
    public class ExcelExportHelper 
    {
        #region "EPPlus"
        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable<T>(List<T> data,string[] columns)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }  

            List<string>  listStr = new List<string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                int count = 0;
                for (int i = 0; i < columns.Length; i++)
			        {
                        if (columns[i] == column.ColumnName)
                        {
                            count =1;
                        }
			        }
                if (count == 0)
                {
                    listStr.Add(column.ColumnName);
                }
            }

            for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn dc = dataTable.Columns[i];
                for (int j = 0; j < listStr.Count; j++)
                {
                    if (listStr[j].ToUpper() == dc.ColumnName.ToUpper())
                    {
                        dataTable.Columns.Remove(dc);
                    }
                }
                
            }  
           return dataTable;
        }

        //public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        //{

        //    byte[] result = null;
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
        //        int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

        //        if (showSrNo)
        //        {
        //            DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
        //            dataColumn.SetOrdinal(0);
        //            int index = 1;
        //            foreach (DataRow item in dataTable.Rows)
        //            {
        //                item[0] = index;
        //                index++;
        //            }
        //        }


        //        // add the content into the Excel file
        //        workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

        //        // autofit width of cells with small content
        //        int columnIndex = 1;
        //        //foreach (DataColumn column in dataTable.Columns)
        //        //{
        //        //    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
        //        //    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
        //        //    if (maxLength < 150)
        //        //    {
        //        //        workSheet.Column(columnIndex).AutoFit();
        //        //    }


        //        //    columnIndex++;
        //        //}

        //        // format header - bold, yellow on black
        //        using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
        //        {
        //            r.Style.Font.Color.SetColor(System.Drawing.Color.White);
        //            r.Style.Font.Bold = true;
        //            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
        //        }

        //        // format cells - add borders
        //        using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
        //        {
        //            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

        //            r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
        //            r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        //            r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
        //            r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
        //        }

        //        // removed ignored columns
        //        for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
        //        {
        //            if (i == 0 && showSrNo)
        //            {
        //                continue;
        //            }
        //            if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
        //            {
        //                workSheet.DeleteColumn(i + 1);
        //            }
        //        }

        //        if (!String.IsNullOrEmpty(heading))
        //        {
        //            workSheet.Cells["A1"].Value = heading;
        //            workSheet.Cells["A1"].Style.Font.Size = 20;

        //            workSheet.InsertColumn(1, 1);
        //            workSheet.InsertRow(1, 1);
        //            workSheet.Column(1).Width = 5;
        //        }

        //        result = package.GetAsByteArray();
        //    }

        //    return result;
        //}

        //public static byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        //{
        //    return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        //}
        #endregion
        #region "NPOI"
        public static void WriteExcelWithNPOI(DataTable dt, String extension, string fileName)
        {            
            string pathExcel = "/Content/Download/";
            string pathDownload = System.Web.Hosting.HostingEnvironment.MapPath(pathExcel + fileName);
                        
            NPOI.SS.UserModel.IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
                
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);
            //set background color
            //ICellStyle style = workbook.CreateCellStyle();
            //style.FillBackgroundColor = IndexedColors.LIGHT_BLUE.Index;
            //style.FillPattern = FillPatternType.FINE_DOTS;

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
                //cell.CellStyle = style;
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;                      
            using (var exportData = new MemoryStream())
            {
                response.Clear();
                workbook.Write(exportData);
                
                if (extension == "xlsx") //xlsx file format
                {
                    response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                    response.BinaryWrite(exportData.ToArray());
                    //Save excel
                    System.IO.File.WriteAllBytes(pathDownload, exportData.ToArray());
                    
                }
                else if (extension == "xls")  //xls file format
                {
                    response.ContentType = "application/vnd.ms-excel";
                    response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                    response.BinaryWrite(exportData.GetBuffer());
                    //Save excel
                    System.IO.File.WriteAllBytes(pathDownload, exportData.ToArray());                    
                    
                }
                
                response.End();
            }
        }

        #endregion
    }
}
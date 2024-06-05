using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using RPA.EXRATE.TOOL.Models;

namespace RPA.EXRATE.TOOL
{
    public static class FileHelper
    {
        public static string parsePdfToText(string pdfFilePath)
        {
            string text = String.Empty;
            try
            {
                // Create a new PDF parser
                using (PdfReader reader = new PdfReader(pdfFilePath))
                {
                    // Extract text from each page
                    for (int page = 1; page <= reader.NumberOfPages; page++)
                    {
                        text += PdfTextExtractor.GetTextFromPage(reader, page);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return text;
        }
        public static Dictionary<string, string> getTableFromText(string text)
        {
            var result = new Dictionary<string, string>();
            result["table"] = String.Empty;
            string inputText = Regex.Replace(text, "\n", Environment.NewLine);
            string pattern = @"MAJOR\sMINOR\sBUYING\sOD\sBUYING\sTT\sSELLING\sTT\/OD\s\s([\s\S]*?)\s^[A-Z]{1}[^A-Z]{2}";
            RegexOptions options = RegexOptions.Multiline;
            MatchCollection matches = Regex.Matches(inputText, pattern, options);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    if (groups.Count > 1)
                    {
                        string matchValue = groups[1].Value;
                        /// Lấy các giá trị trong bảng
                        matchValue = Regex.Replace(matchValue, @"(\d) ([,\.\d])", "$1$2");

                        matchValue = matchValue.Replace("Not applicable", "Notapplicable");
                        matchValue = Regex.Replace(matchValue, @" +", "|");
                        matchValue = matchValue.Replace("Notapplicable", "");
                       
                        result["table"] += matchValue;
                    }
                    result["table"] += "\n";
                }

                /// Lấy các rate
                pattern = @"(\d{1,3}(,\d{3}))";
                matches = Regex.Matches(inputText, pattern);
                result["CeilingRate"] = matches[0].Groups[1].Value;
                result["SvbRate"] = matches[1].Groups[1].Value;
                result["FloorRate"] = matches[2].Groups[1].Value;

                /// Match version + date
                pattern = @"Version: (\d+)\s";
                matches = Regex.Matches(inputText, pattern);
                result["Version"] = matches[0].Groups[1].Value;

                pattern = @"Date: (\d{1,2} [A-z]{3} \d{4})\s";
                matches = Regex.Matches(inputText, pattern);
                DateTime date = DateTime.ParseExact(matches[0].Groups[1].Value, "dd MMM yyyy", CultureInfo.InvariantCulture);
                string formattedDate = date.ToString("yyyy-MM-dd");
                result["Date"] = formattedDate;

            }
            else
            {
                throw new Exception("PDF Data missing");
            }

            return result;
        }

        public static string WriteExcelFileFromList(List<ExchangeRate> exrates, string path)
        {
            try
            {
                XSSFWorkbook wb = new XSSFWorkbook();
                ISheet sheet = wb.CreateSheet();

                var row0 = sheet.CreateRow(0);

                // Ghi tên cột ở row 0
                var row1 = sheet.CreateRow(0);
                row1.CreateCell(0).SetCellValue("ExchangeDate");
                row1.CreateCell(1).SetCellValue("Version");
                row1.CreateCell(2).SetCellValue("MajorCurrency");
                row1.CreateCell(3).SetCellValue("MinorCurrency");
                row1.CreateCell(4).SetCellValue("CeilingRate");
                row1.CreateCell(5).SetCellValue("SvbRate");
                row1.CreateCell(6).SetCellValue("FloorRate");
                row1.CreateCell(7).SetCellValue("BuyingOd");
                row1.CreateCell(8).SetCellValue("BuyingTt");
                row1.CreateCell(9).SetCellValue("AgvRate");
                row1.CreateCell(10).SetCellValue("SellingTtOd");
                // bắt đầu duyệt mảng và ghi tiếp tục
                int rowIndex = 1;
                foreach (var item in exrates)
                {
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRow.CreateCell(0).SetCellValue(item.ExchangeDate);
                    newRow.CreateCell(1).SetCellValue(item.Version);
                    newRow.CreateCell(2).SetCellValue(item.MajorCurrency);
                    newRow.CreateCell(3).SetCellValue(item.MinorCurrency);
                    newRow.CreateCell(4).SetCellValue(item.CeilingRate);
                    newRow.CreateCell(5).SetCellValue(item.SvbRate);
                    newRow.CreateCell(6).SetCellValue(item.FloorRate);
                    newRow.CreateCell(7).SetCellValue(item.BuyingOd == "null" ? String.Empty : item.BuyingOd);
                    newRow.CreateCell(8).SetCellValue(item.BuyingTt);
                    newRow.CreateCell(9).SetCellValue(item.AgvRate);
                    newRow.CreateCell(10).SetCellValue(item.SellingTtOd);
                    // tăng index
                    rowIndex++;
                }

                FileStream fs = new FileStream(path, FileMode.CreateNew);
                wb.Write(fs);

                return "success";
            } catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace TMV.EXRATE.TOOL
{
    public static class PDFHelper
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
            string pattern = @"(?<Currency>[A-Z]{3})\s+(?<BuyingRateVND>[\d\.,]+)\s+(?<BuyingRateUSD>[\d\.,]+)\s+(?<SellingRateVND>[\d\.,]+)\s+(?<SellingRateUSD>[\d\.,]+)|\s+(?<BuyingRateVND>[\d\.,]+)\s+(?<BuyingRateUSD>[\d\.,]+)\s+(?<SellingRateUSD>[\d\.,]+)\s+(?<SellingRateVND>[\d\.,]+)\s+(?<Currency>[A-Z]{3})|(?<Currency>USD)\s+(?<BuyingRateVND>[\d\.,]+)\s+(?<SellingRateVND>[\d\.,]+)";
            RegexOptions options = RegexOptions.Multiline;
            MatchCollection matches = Regex.Matches(inputText, pattern, options);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    if (groups.Count > 1)
                    {
                        string row = 
                            groups["Currency"].Value + "|" + 
                            groups["BuyingRateVND"].Value + "|" + 
                            groups["BuyingRateUSD"].Value + "|" + 
                            groups["SellingRateVND"].Value + "|" + 
                            groups["SellingRateUSD"].Value; 

                        result["table"] += row;
                    }
                    result["table"] += "\n";
                }

                /// Lấy các rate
                pattern = @"(\d{1,3}(,\d{3}))";
                matches = Regex.Matches(inputText, pattern);
                result["SvbRate"] = matches[0].Groups[1].Value;
                result["CeilingRate"] = matches[1].Groups[1].Value;
                result["FloorRate"] = matches[2].Groups[1].Value;

                /// Match version + date
                pattern = @"Version (\d+)\s";
                matches = Regex.Matches(inputText, pattern);
                result["Version"] = matches[0].Groups[1].Value;

                pattern = @"Date: ([0-9]{2} [A-z]+ [0-9]{4})";
                matches = Regex.Matches(inputText, pattern);
                DateTime date = DateTime.ParseExact(matches[0].Groups[1].Value, "dd MMMM yyyy", CultureInfo.InvariantCulture);
                string formattedDate = date.ToString("yyyy-MM-dd");
                result["Date"] = formattedDate;

            }
            else
            {
                throw new Exception("PDF Data missing");
            }

            return result;
        } 
    }
}

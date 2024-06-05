using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TMV.EXRATE.TOOL.Common
{
    public static class Utils
    {
        public static string getFileMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    byte[] checksum = md5.ComputeHash(stream);
                    return BitConverter.ToString(checksum).Replace("-", String.Empty).ToLower();
                }
            }
        }

        public static Boolean IsMailIDReaded(string path, string md5)
        {
            if (!File.Exists(path)) { return false; }
            string text = File.ReadAllText(path);
            return text.Contains(md5);
        }

        public static void WriteMailID(string path, string mailId)
        {
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(mailId);
                }
            } else {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(mailId);
                }
            }     
        }

        public static string BuildHTMLTable(List<Dictionary<string, string>> AllFields)
        {
            string row = String.Empty;
            foreach (var f in AllFields)
            {
                row += "<tr>" +
                       $"<td>{(f["MajorCurrency"] != "null" ? f["MajorCurrency"] : "")}</td>" +
                       $"<td>{(f["MinorCurrency"] != "null" ? f["MinorCurrency"] : "")}</td>" +
                       $"<td>{(f["BuyingOd"] != "null" ? f["BuyingOd"] : "")}</td>" +
                       $"<td>{(f["BuyingTt"] != "null" ? f["BuyingTt"] : "")}</td>" +
                       $"<td>{(f["SellingTtOd"] != "null" ? f["SellingTtOd"] : "")}</td>" +
                       $"<td>{(f["CeilingRate"] != "null" ? f["CeilingRate"] : "")}</td>" +
                       $"<td>{(f["SvbRate"] != "null" ? f["SvbRate"] : "")}</td>" +
                       $"<td>{(f["FloorRate"] != "null" ? f["FloorRate"] : "")}</td>" +
                       $"<td>{(f["ExchangeDate"] != "null" ? f["ExchangeDate"] : "")}</td>" +
                       $"<td>{(f["Version"] != "null" ? f["Version"] : "")}</td>" +
                       $"<td>{(f["AgvRate"] != "null" ? f["AgvRate"] : "")}</td>" +
                           "</tr>";
            }

            return $"<table width=\"100%\" height=\"1%\" cellspacing=\"0\" cellpadding=\"8\" border=\"1\" align=\"\"><tr><th width=\"9%\">MajorCurrency</th><th width=\"9%\">MinorCurrency</th><th width=\"9%\">BuyingOd</th><th width=\"9%\">BuyingTt</th><th width=\"9%\">SellingTtOd</th><th width=\"9%\">CeilingRate</th><th width=\"9%\">SvbRate</th><th width=\"9%\">FloorRate</th><th width=\"9%\">ExchangeDate</th><th width=\"9%\">Version</th><th width=\"9%\">AgvRate</th></tr>{row}</table>";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.EXRATE.TOOL.Models
{
    public class ExchangeRate
    {
        public string ExchangeDate { get;set ; }
        public string Version { get; set; }
        public string MajorCurrency { get; set; }
        public string MinorCurrency { get; set; }

        public string CeilingRate { get; set; }
        public string SvbRate { get; set; }
        public string FloorRate { get; set; }
        public string BuyingOd { get; set; }
        public string BuyingTt { get; set; }
        public string AgvRate { get; set; }
        public string SellingTtOd { get; set; }
        public string Guid { get; set; }
        public string CreationTime { get; set; }
    }   
}

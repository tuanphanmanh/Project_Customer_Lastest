using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMV.ObjectInfo
{
    public class LgaBarScanInfo
    {
        #region "public Members"

        public long Id { get; set; }
        public string ScanType { get; set; }
        public DateTime? ScanDatetime { get; set; }
        public string ScanValue { get; set; }
        public string ScanBackNo { get; set; }
        public string ScanPartNo { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProdLine { get; set; }
        public long RefId { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }

        #endregion

        #region "Constructors"
        public LgaBarScanInfo()
        {
            Id = 0;
            ScanType = string.Empty;
            ScanValue = string.Empty;
            ScanPartNo = string.Empty;
            ScanBackNo = string.Empty;
            UserId = string.Empty;
            UserName = string.Empty;
            ProdLine = string.Empty;
            Status = string.Empty;
            IsActive = string.Empty;
        }
        #endregion 
    }
}



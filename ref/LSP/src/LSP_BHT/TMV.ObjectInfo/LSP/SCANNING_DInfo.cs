using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMV.ObjectInfo
{

    public class SCANNING_DInfor
    {
        #region "Property"
        public Int64 ID { get; set; }
        public Int64 SCANNING_H_ID { get; set; }
        public string SCANNING_ITEM { get; set; }
        public string SCANNING_VALUE { get; set; }
        public DateTime SCANNING_DATETIME { get; set; }
        
        #endregion

        #region "Constants"
        public static readonly string ID_COL = "ID";
        public static readonly string SCANNING_H_ID_COL = "SCANNING_H_ID";
        public static readonly string SCANNING_ITEM_COL = "SCANNING_ITEM";        
        public static readonly string SCANNING_VALUE_COL = "SCANNING_VALUE";    
        public static readonly string SCANNING_DATETIME_COL = "SCANNING_DATETIME";    
        #endregion

        #region "Init Class"
        public SCANNING_DInfor()
        {
        }
        public SCANNING_DInfor(Int64 ID, Int64 SCANNING_ID, string SCANNING_ITEM, string SCANNING_VALUE, DateTime SCANNING_DATETIME)
        {
            this.ID = ID;
            this.SCANNING_H_ID = SCANNING_H_ID;
            this.SCANNING_ITEM = SCANNING_ITEM;
            this.SCANNING_VALUE = SCANNING_VALUE;
            this.SCANNING_DATETIME = SCANNING_DATETIME;            
        }
        #endregion
    }
}
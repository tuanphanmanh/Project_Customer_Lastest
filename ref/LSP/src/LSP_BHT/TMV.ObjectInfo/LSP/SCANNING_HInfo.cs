using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMV.ObjectInfo
{

    public class SCANNING_HInfor
    {
        #region "Property"
        public Int64 ID { get; set; }
        public Int32 USER_ID { get; set; }
        public Int32 PROCESS_ID { get; set; }        
        public DateTime SCANNING_DATETIME { get; set; }
                
        #endregion

        #region "Constants"
        public static readonly string ID_COL = "ID";
        public static readonly string USER_ID_COL = "USER_ID";
        public static readonly string PROCESS_ID_COL = "PROCESS_ID";        
        public static readonly string DATETIME_COL = "SCANNING_DATETIME";
               
        #endregion

        #region "Init Class"
        public SCANNING_HInfor()
        {
        }
        public SCANNING_HInfor(Int64 ID, Int32 USER_ID, Int32 PROCESS_ID, Int32 PERIOD_ID, DateTime SCANNING_DATETIME)
        {
            this.ID = ID;
            this.USER_ID = USER_ID;            
            this.SCANNING_DATETIME = SCANNING_DATETIME;
            this.PROCESS_ID = PROCESS_ID;            
        }
        #endregion
    }
}
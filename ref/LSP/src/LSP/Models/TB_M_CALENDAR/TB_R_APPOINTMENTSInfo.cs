using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_CALENDAR
{
    public class TB_R_APPOINTMENTSInfo
    {
     
        #region "Public Members"
            public Int32 ID { get; set; }
            public int ROW_NO { get; set; }
            public int TYPE { get; set; }        
            public DateTime? STARTDATE { get; set; }
            public DateTime? ENDDATE { get; set; }
            public int ALLDAY { get; set; }                      
            public string SUBJECT { get; set; }
            public string LOCATION { get; set; }
            public string DESCRIPTION { get; set; }
            public int STATUS { get; set; }
            public int LABEL { get; set; }
            public Int32 RESOURCEID { get; set; }      
            public string REMINDERINFO { get; set; }
            public string RECURRENCEINFO { get; set; }
        #endregion

        #region "Constructors"
            public TB_R_APPOINTMENTSInfo()
            {
                ID = 0;
                ROW_NO = 0;
                TYPE = 0;
                STARTDATE = null;
                ENDDATE = null;
                ALLDAY = 0;
                SUBJECT = string.Empty;
                LOCATION = string.Empty;
                DESCRIPTION = string.Empty;
                STATUS = 0;
                LABEL = 0;
                RESOURCEID = 0;
                REMINDERINFO = string.Empty;            
                RECURRENCEINFO = string.Empty;            
            }        
        #endregion
    }   
}
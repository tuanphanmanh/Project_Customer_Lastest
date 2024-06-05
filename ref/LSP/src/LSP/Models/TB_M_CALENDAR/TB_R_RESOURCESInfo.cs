using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_CALENDAR
{   
    public class TB_R_RESOURCESInfo
    {
     
        #region "Public Members"
            public Int32 ID { get; set; }
            public int ROW_NO { get; set; }
            public int RESOURCEID  { get; set; }        
            public string RESOURCENAME { get; set; }
            public int COLOR { get; set; }                                                                
            public string DECRIPTION { get; set; }           
        #endregion

        #region "Constructors"
            public TB_R_RESOURCESInfo()
            {
                ID = 0;
                ROW_NO = 0;
                RESOURCEID = 0;
                RESOURCENAME = string.Empty;               
                COLOR = 0;
                DECRIPTION = string.Empty;            
            }        
        #endregion
    }   
}
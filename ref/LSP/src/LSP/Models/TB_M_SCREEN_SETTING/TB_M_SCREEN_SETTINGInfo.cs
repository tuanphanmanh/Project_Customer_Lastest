using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LSP.Models.TB_M_SCREEN_SETTING
{
    public class TB_M_SCREEN_SETTINGInfo
    {
         #region "public Members"
            public long ID { get; set; }
            public long ROW_NO { get; set; }            
            public string SCREEN_NAME { get; set; }
            public string SCREEN_TYPE { get; set; }
            public string SCREEN_VALUE { get; set; }           
            public string DESCRIPTION { get; set; }                         
            public string IS_ACTIVE { get; set; }
            public bool? IS_ACTIVE_BOL
            {
                get
                {
                    return IS_ACTIVE == "Y" ? true : false;
                }
                set
                {
                    if (value == true) IS_ACTIVE = "Y";
                    else IS_ACTIVE = "N";
                }
            }
           
            public string CREATED_BY { get; set; }
            public DateTime? CREATED_DATE { get; set; }
            public string CREATED_DATE_Str_DDMMYYYY
            {
                get
                {
                    try
                    {
                        return string.Format("{0:dd/MM/yyyy}", CREATED_DATE);
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                }
            }
            public string UPDATED_BY { get; set; }
            public DateTime? UPDATED_DATE { get; set; }
            public string UPDATED_DATE_Str_DDMMYYYY
            {
                get
                {
                    try
                    {
                        return string.Format("{0:dd/MM/yyyy}", UPDATED_DATE);
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                }
            }

            public int BARCODE_ID { get; set; }

        #endregion

        #region "Constructors"
        public TB_M_SCREEN_SETTINGInfo()
        {
            ID = 0;
            ROW_NO = 0;
            SCREEN_NAME = string.Empty;
            SCREEN_TYPE = string.Empty;
            SCREEN_VALUE = string.Empty;        
            DESCRIPTION = string.Empty;
            IS_ACTIVE = string.Empty;            
            CREATED_BY = string.Empty;
            CREATED_DATE = null;
            UPDATED_BY = string.Empty;
            UPDATED_DATE = null;
            BARCODE_ID = 0;
        }
       
        #endregion
    }
}
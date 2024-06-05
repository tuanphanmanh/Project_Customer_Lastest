using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_CALENDAR
{
    public class TB_M_CALENDAR_PIVOTInfo
    {
        #region "Public Members"
        public int ID { get; set; }
        public int ROW_NO { get; set; }
        public string SUPPLIER_CODE { get; set; }

        public string DAY_1 { get; set; }
        public string DAY_2 { get; set; }
        public string DAY_3 { get; set; }
        public string DAY_4 { get; set; }
        public string DAY_5 { get; set; }
        public string DAY_6 { get; set; }
        public string DAY_7 { get; set; }
        public string DAY_8 { get; set; }
        public string DAY_9 { get; set; }
        public string DAY_10 { get; set; }
        public string DAY_11 { get; set; }
        public string DAY_12 { get; set; }
        public string DAY_13 { get; set; }
        public string DAY_14 { get; set; }
        public string DAY_15 { get; set; }
        public string DAY_16 { get; set; }
        public string DAY_17 { get; set; }
        public string DAY_18 { get; set; }
        public string DAY_19 { get; set; }
        public string DAY_20 { get; set; }
        public string DAY_21 { get; set; }
        public string DAY_22 { get; set; }
        public string DAY_23 { get; set; }
        public string DAY_24 { get; set; }
        public string DAY_25 { get; set; }
        public string DAY_26 { get; set; }
        public string DAY_27 { get; set; }
        public string DAY_28 { get; set; }
        public string DAY_29 { get; set; }
        public string DAY_30 { get; set; }
        public string DAY_31 { get; set; }


        public DateTime? WORKING_MONTH { get; set; }
        public string WORKING_MONTH_Str_DDMMYYYY 
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", WORKING_MONTH);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public string WORKING_MONTH_Str_MMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:MM/yyyy}", WORKING_MONTH);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        
        #endregion

         
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PRODUCTION_PLAN_D_W
{
    public class TB_R_PRODUCTION_PLAN_D_WInfo
    {
        #region "Public Members"
		public int ID { get; set; }
        public int Row_No { get; set; }
        public string CFC { get; set; }
        public string KATASHIKI { get; set; }
        public string PROD_SFX { get; set; }
        public string LOT_NO { get; set; }
        public int NO_IN_LOT { get; set; }
        public string BODY_NO { get; set; }
        public string EXT_COLOR { get; set; }
        public string VIN_NO { get; set; }
        public string PRODUCTION_LINE { get; set; }
        public string SHIFT { get; set; }
        public string SEQUENCE_NO { get; set; }
        public DateTime? WORKING_DATE { get; set; }
        public string WORKING_DATE_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", WORKING_DATE);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public int NO_IN_DAY { get; set; }
        public DateTime? W_IN_DATE_PLAN { get; set; }
        public string W_IN_DATE_PLAN_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", W_IN_DATE_PLAN);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public TimeSpan? W_IN_TIME_PLAN { get; set; }
        public DateTime? W_IN_DATE_ACTUAL { get; set; }
        public string W_IN_DATE_ACTUAL_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", W_IN_DATE_ACTUAL);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public TimeSpan? W_IN_TIME_ACTUAL { get; set; }
        public DateTime? W_OUT_DATE_PLAN { get; set; }
        public string W_OUT_DATE_PLAN_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", W_OUT_DATE_PLAN);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public TimeSpan? W_OUT_TIME_PLAN { get; set; }
        public DateTime? W_OUT_DATE_ACTUAL { get; set; }
        public string W_OUT_DATE_ACTUAL_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", W_OUT_DATE_ACTUAL);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public TimeSpan? W_OUT_TIME_ACTUAL { get; set; }
        public int VERSION_NO { get; set; }
        public string IS_NQC_PROCESSED { get; set; }
        public string IS_ACTIVE { get; set; }
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

        public DateTime? PRODUCTION_MONTH { get; set; }
        public string PRODUCTION_MONTH_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", PRODUCTION_MONTH);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public string PRODUCTION_MONTH_Str_MMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:MM/yyyy}", PRODUCTION_MONTH);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

		#endregion

		#region "Constructors"
		public TB_R_PRODUCTION_PLAN_D_WInfo() 
		{
			ID = 0;
            Row_No = 0;
			CFC = string.Empty;
            KATASHIKI = string.Empty;
            PROD_SFX = string.Empty;
            LOT_NO = string.Empty;
            NO_IN_LOT = 0;
            BODY_NO = string.Empty;
            EXT_COLOR = string.Empty;
            VIN_NO = string.Empty;
            PRODUCTION_LINE = string.Empty;
            SHIFT = string.Empty;
            SEQUENCE_NO = string.Empty;
            WORKING_DATE = null;
            NO_IN_DAY = 0;
            W_IN_DATE_PLAN = null;
            W_IN_TIME_PLAN = null;
            W_IN_DATE_ACTUAL = null;
            W_IN_TIME_ACTUAL = null;
            W_OUT_DATE_PLAN = null;
            W_OUT_TIME_PLAN = null;
            W_OUT_DATE_ACTUAL = null;
            W_OUT_TIME_ACTUAL = null;
            VERSION_NO = 0;
            IS_NQC_PROCESSED = string.Empty;
            IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
            PRODUCTION_MONTH = null;
		}
        #endregion
    }
}
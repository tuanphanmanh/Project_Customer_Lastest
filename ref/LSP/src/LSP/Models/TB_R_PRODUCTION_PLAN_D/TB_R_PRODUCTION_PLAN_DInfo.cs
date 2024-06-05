using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PRODUCTION_PLAN_D
{
    public class TB_R_PRODUCTION_PLAN_DInfo
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
        public DateTime? A_IN_DATE_PLAN { get; set; }
        public string A_IN_DATE_PLAN_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", A_IN_DATE_PLAN);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public TimeSpan? A_IN_TIME_PLAN { get; set; }
        public DateTime? A_IN_DATE_ACTUAL { get; set; }
        public string A_IN_DATE_ACTUAL_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", A_IN_DATE_ACTUAL);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public TimeSpan? A_IN_TIME_ACTUAL { get; set; }
        public DateTime? A_OUT_DATE_PLAN { get; set; }
        public string A_OUT_DATE_PLAN_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", A_OUT_DATE_PLAN);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public TimeSpan? A_OUT_TIME_PLAN { get; set; }
        public DateTime? A_OUT_DATE_ACTUAL { get; set; }
        public string A_OUT_DATE_ACTUAL_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", A_OUT_DATE_ACTUAL);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public TimeSpan? A_OUT_TIME_ACTUAL { get; set; }
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
		public TB_R_PRODUCTION_PLAN_DInfo() 
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
            A_IN_DATE_PLAN = null;
            A_IN_TIME_PLAN = null;
            A_IN_DATE_ACTUAL = null;
            A_IN_TIME_ACTUAL = null;
            A_OUT_DATE_PLAN = null;
            A_OUT_TIME_PLAN = null;
            A_OUT_DATE_ACTUAL = null;
            A_OUT_TIME_ACTUAL = null;
            VERSION_NO = 0;
            IS_NQC_PROCESSED = string.Empty;
            IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
            PRODUCTION_MONTH = null;
		}

        public TB_R_PRODUCTION_PLAN_DInfo(int ID, int Row_No, string CFC, string KATASHIKI, string PROD_SFX, string LOT_NO, int NO_IN_LOT, string BODY_NO, string EXT_COLOR, string VIN_NO, string PRODUCTION_LINE, string SHIFT,
            string SEQUENCE_NO, DateTime? WORKING_DATE, int NO_IN_DAY, DateTime? A_IN_DATE_PLAN, TimeSpan? A_IN_TIME_PLAN, DateTime? A_IN_DATE_ACTUAL, TimeSpan? A_IN_TIME_ACTUAL, DateTime? A_OUT_DATE_PLAN, TimeSpan? A_OUT_TIME_PLAN,
            DateTime? A_OUT_DATE_ACTUAL, TimeSpan? A_OUT_TIME_ACTUAL, int VERSION_NO, string IS_NQC_PROCESSED, string IS_ACTIVE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE,
            DateTime PRODUCTION_MONTH)
		{
			this.ID = ID;
			this.Row_No = Row_No;
			this.CFC = CFC;
			this.KATASHIKI = KATASHIKI;
			this.PROD_SFX = PROD_SFX;
			this.LOT_NO = LOT_NO;
			this.NO_IN_LOT = NO_IN_LOT;
			this.BODY_NO = BODY_NO;
            this.EXT_COLOR = EXT_COLOR;
            this.VIN_NO = VIN_NO;
            this.PRODUCTION_LINE = PRODUCTION_LINE;
            this.SHIFT = SHIFT;
            this.SEQUENCE_NO = SEQUENCE_NO;
			this.WORKING_DATE = WORKING_DATE;
            this.NO_IN_DAY = NO_IN_DAY;
            this.A_IN_DATE_PLAN = A_IN_DATE_PLAN;
            this.A_IN_TIME_PLAN = A_IN_TIME_PLAN;
            this.A_IN_DATE_ACTUAL = A_IN_DATE_ACTUAL;
            this.A_IN_TIME_ACTUAL = A_IN_TIME_ACTUAL;
			this.A_OUT_DATE_PLAN = A_OUT_DATE_PLAN;
            this.A_OUT_TIME_PLAN = A_OUT_TIME_PLAN;
            this.A_OUT_DATE_ACTUAL = A_OUT_DATE_ACTUAL;
            this.A_OUT_TIME_ACTUAL = A_OUT_TIME_ACTUAL;
			this.VERSION_NO = VERSION_NO;
            this.IS_NQC_PROCESSED = IS_NQC_PROCESSED;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
            this.PRODUCTION_MONTH = PRODUCTION_MONTH;
		}
        #endregion
    }
}
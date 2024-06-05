using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_NQC_RESULT_M
{
    public class TB_R_NQC_RESULT_MInfo
    {
        #region "Public Members"

		public int ID { get; set; }
        public int Row_No { get; set; }
        public string CFC { get; set; }
		public string PART_NO { get; set; }
        public string PROD_SFX { get; set; }
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
        public string PARTS_MATCHING_KEY { get; set; }
        public int DAILY_QTY01 { get; set; }
        public int DAILY_QTY02 { get; set; }
        public int DAILY_QTY03 { get; set; }
        public int DAILY_QTY04 { get; set; }
        public int DAILY_QTY05 { get; set; }
        public int DAILY_QTY06 { get; set; }
        public int DAILY_QTY07 { get; set; }
        public int DAILY_QTY08 { get; set; }
        public int DAILY_QTY09 { get; set; }
        public int DAILY_QTY10 { get; set; }
        public int DAILY_QTY11 { get; set; }
        public int DAILY_QTY12 { get; set; }
        public int DAILY_QTY13 { get; set; }
        public int DAILY_QTY14 { get; set; }
        public int DAILY_QTY15 { get; set; }
        public int DAILY_QTY16 { get; set; }
        public int DAILY_QTY17 { get; set; }
        public int DAILY_QTY18 { get; set; }
        public int DAILY_QTY19 { get; set; }
        public int DAILY_QTY20 { get; set; }
        public int DAILY_QTY21 { get; set; }
        public int DAILY_QTY22 { get; set; }
        public int DAILY_QTY23 { get; set; }
        public int DAILY_QTY24 { get; set; }
        public int DAILY_QTY25 { get; set; }
        public int DAILY_QTY26 { get; set; }
        public int DAILY_QTY27 { get; set; }
        public int DAILY_QTY28 { get; set; }
        public int DAILY_QTY29 { get; set; }
        public int DAILY_QTY30 { get; set; }
        public int DAILY_QTY31 { get; set; }
        public int TOTAL_QTY { get; set; }
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
				catch(Exception ex)
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
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		#endregion

		#region "Constructors"
		public TB_R_NQC_RESULT_MInfo() 
		{
			ID = 0;
            Row_No = 0;
            CFC = string.Empty;
			PART_NO = string.Empty;
            PROD_SFX = string.Empty;
            PRODUCTION_MONTH = null;
            PARTS_MATCHING_KEY = string.Empty;
            DAILY_QTY01 = 0;
            DAILY_QTY02 = 0;
            DAILY_QTY03 = 0;
            DAILY_QTY04 = 0;
            DAILY_QTY05 = 0;
            DAILY_QTY06 = 0;
            DAILY_QTY07 = 0;
            DAILY_QTY08 = 0;
            DAILY_QTY09 = 0;
            DAILY_QTY10 = 0;
            DAILY_QTY11 = 0;
            DAILY_QTY12 = 0;
            DAILY_QTY13 = 0;
            DAILY_QTY14 = 0;
            DAILY_QTY15 = 0;
            DAILY_QTY16 = 0;
            DAILY_QTY17 = 0;
            DAILY_QTY18 = 0;
            DAILY_QTY19 = 0;
            DAILY_QTY20 = 0;
            DAILY_QTY21 = 0;
            DAILY_QTY22 = 0;
            DAILY_QTY23 = 0;
            DAILY_QTY24 = 0;
            DAILY_QTY25 = 0;
            DAILY_QTY26 = 0;
            DAILY_QTY27 = 0;
            DAILY_QTY28 = 0;
            DAILY_QTY29 = 0;
            DAILY_QTY30 = 0;
            DAILY_QTY31 = 0;
            TOTAL_QTY = 0;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}

        public TB_R_NQC_RESULT_MInfo(int ID, int Row_No, string CFC, string PART_NO, string PROD_SFX, DateTime PRODUCTION_MONTH, string PARTS_MATCHING_KEY,
            int DAILY_QTY01, int DAILY_QTY02, int DAILY_QTY03, int DAILY_QTY04, int DAILY_QTY05, int DAILY_QTY06, int DAILY_QTY07, int DAILY_QTY08, int DAILY_QTY09,
            int DAILY_QTY10, int DAILY_QTY11, int DAILY_QTY12, int DAILY_QTY13, int DAILY_QTY14, int DAILY_QTY15, int DAILY_QTY16, int DAILY_QTY17, int DAILY_QTY18,
            int DAILY_QTY19, int DAILY_QTY20, int DAILY_QTY21, int DAILY_QTY22, int DAILY_QTY23, int DAILY_QTY24, int DAILY_QTY25, int DAILY_QTY26, int DAILY_QTY27,
            int DAILY_QTY28, int DAILY_QTY29, int DAILY_QTY30, int DAILY_QTY31, int TOTAL_QTY,  string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
            this.Row_No = Row_No;
            this.CFC = CFC;
            this.PART_NO = PART_NO;
            this.PROD_SFX = PROD_SFX;
            this.PRODUCTION_MONTH = PRODUCTION_MONTH;
            this.PARTS_MATCHING_KEY = PARTS_MATCHING_KEY;
            this.DAILY_QTY01 = DAILY_QTY01;
            this.DAILY_QTY02 = DAILY_QTY02;
            this.DAILY_QTY03 = DAILY_QTY03;
            this.DAILY_QTY04 = DAILY_QTY04;
            this.DAILY_QTY05 = DAILY_QTY05;
            this.DAILY_QTY06 = DAILY_QTY06;
            this.DAILY_QTY07 = DAILY_QTY07;
            this.DAILY_QTY08 = DAILY_QTY08;
            this.DAILY_QTY09 = DAILY_QTY09;
            this.DAILY_QTY10 = DAILY_QTY10;
            this.DAILY_QTY11 = DAILY_QTY11;
            this.DAILY_QTY12 = DAILY_QTY12;
            this.DAILY_QTY13 = DAILY_QTY13;
            this.DAILY_QTY14 = DAILY_QTY14;
            this.DAILY_QTY15 = DAILY_QTY15;
            this.DAILY_QTY16 = DAILY_QTY16;
            this.DAILY_QTY17 = DAILY_QTY17;
            this.DAILY_QTY18 = DAILY_QTY18;
            this.DAILY_QTY19 = DAILY_QTY19;
            this.DAILY_QTY20 = DAILY_QTY20;
            this.DAILY_QTY21 = DAILY_QTY21;
            this.DAILY_QTY22 = DAILY_QTY22;
            this.DAILY_QTY23 = DAILY_QTY23;
            this.DAILY_QTY24 = DAILY_QTY24;
            this.DAILY_QTY25 = DAILY_QTY25;
            this.DAILY_QTY26 = DAILY_QTY26;
            this.DAILY_QTY27 = DAILY_QTY27;
            this.DAILY_QTY28 = DAILY_QTY28;
            this.DAILY_QTY29 = DAILY_QTY29;
            this.DAILY_QTY30 = DAILY_QTY30;
            this.DAILY_QTY31 = DAILY_QTY31;
            this.TOTAL_QTY = TOTAL_QTY;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}
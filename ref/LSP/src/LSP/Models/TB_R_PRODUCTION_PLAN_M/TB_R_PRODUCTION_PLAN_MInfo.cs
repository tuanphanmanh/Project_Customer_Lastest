using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PRODUCTION_PLAN_M
{
    public class TB_R_PRODUCTION_PLAN_MInfo
    {
        #region "Public Members"

		public int ID { get; set; }
        public int Row_No { get; set; }
        public string CFC { get; set; }
		public string KATASHIKI { get; set; }
        public string PROD_SFX { get; set; }
        public string INT_COLOR { get; set; }
        public string EXT_COLOR { get; set; }
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
        public int LO_VOLUME { get; set; }
        public int LO_VOLUME_DAY01 { get; set; }
        public int LO_VOLUME_DAY02 { get; set; }
        public int LO_VOLUME_DAY03 { get; set; }
        public int LO_VOLUME_DAY04 { get; set; }
        public int LO_VOLUME_DAY05 { get; set; }
        public int LO_VOLUME_DAY06 { get; set; }
        public int LO_VOLUME_DAY07 { get; set; }
        public int LO_VOLUME_DAY08 { get; set; }
        public int LO_VOLUME_DAY09 { get; set; }
        public int LO_VOLUME_DAY10 { get; set; }
        public int LO_VOLUME_DAY11 { get; set; }
        public int LO_VOLUME_DAY12 { get; set; }
        public int LO_VOLUME_DAY13 { get; set; }
        public int LO_VOLUME_DAY14 { get; set; }
        public int LO_VOLUME_DAY15 { get; set; }
        public int LO_VOLUME_DAY16 { get; set; }
        public int LO_VOLUME_DAY17 { get; set; }
        public int LO_VOLUME_DAY18 { get; set; }
        public int LO_VOLUME_DAY19 { get; set; }
        public int LO_VOLUME_DAY20 { get; set; }
        public int LO_VOLUME_DAY21 { get; set; }
        public int LO_VOLUME_DAY22 { get; set; }
        public int LO_VOLUME_DAY23 { get; set; }
        public int LO_VOLUME_DAY24 { get; set; }
        public int LO_VOLUME_DAY25 { get; set; }
        public int LO_VOLUME_DAY26 { get; set; }
        public int LO_VOLUME_DAY27 { get; set; }
        public int LO_VOLUME_DAY28 { get; set; }
        public int LO_VOLUME_DAY29 { get; set; }
        public int LO_VOLUME_DAY30 { get; set; }
        public int LO_VOLUME_DAY31 { get; set; }
        public string IS_NQC_REQ_PROCESSED { get; set; }
        public string IS_NQC_RES_PROCESSED { get; set; }
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


        public string GUID { get; set; }
        public string PART_NO { get; set; }
        public string COLOR_SFX { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public int BOX_SIZE { get; set; }
        public int QTY_PER_VEHICLE { get; set; }
        public int LO_VOLUME_FC { get; set; }

		#endregion

		#region "Constructors"
		public TB_R_PRODUCTION_PLAN_MInfo() 
		{
			ID = 0;
            Row_No = 0;
            CFC = string.Empty;
			KATASHIKI = string.Empty;
            PROD_SFX = string.Empty;
            INT_COLOR = string.Empty;
            EXT_COLOR = string.Empty;
            PRODUCTION_MONTH = null;
            LO_VOLUME = 0;
            LO_VOLUME_DAY01 = 0;
            LO_VOLUME_DAY02 = 0;
            LO_VOLUME_DAY03 = 0;
            LO_VOLUME_DAY04 = 0;
            LO_VOLUME_DAY05 = 0;
            LO_VOLUME_DAY06 = 0;
            LO_VOLUME_DAY07 = 0;
            LO_VOLUME_DAY08 = 0;
            LO_VOLUME_DAY09 = 0;
            LO_VOLUME_DAY10 = 0;
            LO_VOLUME_DAY11 = 0;
            LO_VOLUME_DAY12 = 0;
            LO_VOLUME_DAY13 = 0;
            LO_VOLUME_DAY14 = 0;
            LO_VOLUME_DAY15 = 0;
            LO_VOLUME_DAY16 = 0;
            LO_VOLUME_DAY17 = 0;
            LO_VOLUME_DAY18 = 0;
            LO_VOLUME_DAY19 = 0;
            LO_VOLUME_DAY20 = 0;
            LO_VOLUME_DAY21 = 0;
            LO_VOLUME_DAY22 = 0;
            LO_VOLUME_DAY23 = 0;
            LO_VOLUME_DAY24 = 0;
            LO_VOLUME_DAY25 = 0;
            LO_VOLUME_DAY26 = 0;
            LO_VOLUME_DAY27 = 0;
            LO_VOLUME_DAY28 = 0;
            LO_VOLUME_DAY29 = 0;
            LO_VOLUME_DAY30 = 0;
            LO_VOLUME_DAY31 = 0;
            IS_NQC_REQ_PROCESSED = string.Empty;
            IS_NQC_RES_PROCESSED = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
            GUID = string.Empty;
            PART_NO = string.Empty;
            COLOR_SFX = string.Empty;
            SUPPLIER_CODE = string.Empty;
            BOX_SIZE = 0;
            QTY_PER_VEHICLE = 0;
            LO_VOLUME_FC = 0;
		}

        public TB_R_PRODUCTION_PLAN_MInfo(int ID, int Row_No, string CFC, string KATASHIKI, string PROD_SFX, string INT_COLOR, string EXT_COLOR, DateTime PRODUCTION_MONTH,
            int LO_VOLUME, int LO_VOLUME_DAY01, int LO_VOLUME_DAY02, int LO_VOLUME_DAY03, int LO_VOLUME_DAY04, int LO_VOLUME_DAY05, int LO_VOLUME_DAY06, int LO_VOLUME_DAY07, int LO_VOLUME_DAY08,
            int LO_VOLUME_DAY09, int LO_VOLUME_DAY10, int LO_VOLUME_DAY11, int LO_VOLUME_DAY12, int LO_VOLUME_DAY13, int LO_VOLUME_DAY14, int LO_VOLUME_DAY15, int LO_VOLUME_DAY16, int LO_VOLUME_DAY17,
            int LO_VOLUME_DAY18, int LO_VOLUME_DAY19, int LO_VOLUME_DAY20, int LO_VOLUME_DAY21, int LO_VOLUME_DAY22, int LO_VOLUME_DAY23, int LO_VOLUME_DAY24, int LO_VOLUME_DAY25, int LO_VOLUME_DAY26,
            int LO_VOLUME_DAY27, int LO_VOLUME_DAY28, int LO_VOLUME_DAY29, int LO_VOLUME_DAY30, int LO_VOLUME_DAY31, string IS_NQC_REQ_PROCESSED, string IS_NQC_RES_PROCESSED, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
            this.Row_No = Row_No;
            this.CFC = CFC;
            this.KATASHIKI = KATASHIKI;
            this.PROD_SFX = PROD_SFX;
            this.INT_COLOR = INT_COLOR;
            this.EXT_COLOR = EXT_COLOR;
            this.PRODUCTION_MONTH = PRODUCTION_MONTH;
            this.LO_VOLUME = LO_VOLUME;
            this.LO_VOLUME_DAY01 = LO_VOLUME_DAY01;
            this.LO_VOLUME_DAY02 = LO_VOLUME_DAY02;
            this.LO_VOLUME_DAY03 = LO_VOLUME_DAY03;
            this.LO_VOLUME_DAY04 = LO_VOLUME_DAY04;
            this.LO_VOLUME_DAY05 = LO_VOLUME_DAY05;
            this.LO_VOLUME_DAY06 = LO_VOLUME_DAY06;
            this.LO_VOLUME_DAY07 = LO_VOLUME_DAY07;
            this.LO_VOLUME_DAY08 = LO_VOLUME_DAY08;
            this.LO_VOLUME_DAY09 = LO_VOLUME_DAY09;
            this.LO_VOLUME_DAY10 = LO_VOLUME_DAY10;
            this.LO_VOLUME_DAY11 = LO_VOLUME_DAY11;
            this.LO_VOLUME_DAY12 = LO_VOLUME_DAY12;
            this.LO_VOLUME_DAY13 = LO_VOLUME_DAY13;
            this.LO_VOLUME_DAY14 = LO_VOLUME_DAY14;
            this.LO_VOLUME_DAY15 = LO_VOLUME_DAY15;
            this.LO_VOLUME_DAY16 = LO_VOLUME_DAY16;
            this.LO_VOLUME_DAY17 = LO_VOLUME_DAY17;
            this.LO_VOLUME_DAY18 = LO_VOLUME_DAY18;
            this.LO_VOLUME_DAY19 = LO_VOLUME_DAY19;
            this.LO_VOLUME_DAY20 = LO_VOLUME_DAY20;
            this.LO_VOLUME_DAY21 = LO_VOLUME_DAY21;
            this.LO_VOLUME_DAY22 = LO_VOLUME_DAY22;
            this.LO_VOLUME_DAY23 = LO_VOLUME_DAY23;
            this.LO_VOLUME_DAY24 = LO_VOLUME_DAY24;
            this.LO_VOLUME_DAY25 = LO_VOLUME_DAY25;
            this.LO_VOLUME_DAY26 = LO_VOLUME_DAY26;
            this.LO_VOLUME_DAY27 = LO_VOLUME_DAY27;
            this.LO_VOLUME_DAY28 = LO_VOLUME_DAY28;
            this.LO_VOLUME_DAY29 = LO_VOLUME_DAY29;
            this.LO_VOLUME_DAY30 = LO_VOLUME_DAY30;
            this.LO_VOLUME_DAY31 = LO_VOLUME_DAY31;
            this.IS_NQC_REQ_PROCESSED = IS_NQC_REQ_PROCESSED;
            this.IS_NQC_RES_PROCESSED = IS_NQC_RES_PROCESSED;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}
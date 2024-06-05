using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_VEHICLE_MASTER
{
    public class TB_M_VEHICLE_MASTERInfo
    {
        #region "Public Members"
		public int ID { get; set; }
        public int Row_No { get; set; }
		public int MODEL_ID { get; set; }
        public string NAME { get; set; }
		public string CFC { get; set; }
        public string PROJECT_CODE { get; set; }
        public string KATASHIKI { get; set; }
        public string PROD_SFX { get; set; }
        public string MKT_SFX { get; set; }
        public string GRADE_MARK { get; set; }
        public string START_LOT { get; set; }
        public DateTime? START_PROD_DATE { get; set; }
        public string START_PROD_DATE_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", START_PROD_DATE);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public string END_LOT { get; set; }
        public DateTime? END_PROD_DATE { get; set; }
        public string END_PROD_DATE_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", END_PROD_DATE);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
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
		public TB_M_VEHICLE_MASTERInfo() 
		{
			ID = 0;
            Row_No = 0;
            MODEL_ID = 0;
			CFC = string.Empty;
			PROJECT_CODE = string.Empty;
            KATASHIKI = string.Empty;
            PROD_SFX = string.Empty;
            MKT_SFX = string.Empty;
            GRADE_MARK = string.Empty;
            START_LOT = string.Empty;
            START_PROD_DATE = null;
            END_LOT = string.Empty;
            END_PROD_DATE = null;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}

        public TB_M_VEHICLE_MASTERInfo(int id, int Row_No, int MODEL_ID, string CFC, string PROJECT_CODE, string KATASHIKI, string PROD_SFX, string MKT_SFX, string GRADE_MARK,
            string START_LOT, DateTime START_PROD_DATE, string END_LOT, DateTime END_PROD_DATE, string IS_ACTIVE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
            this.Row_No = Row_No;
            this.MODEL_ID = MODEL_ID;
			this.CFC = CFC;
            this.PROJECT_CODE = PROJECT_CODE;
            this.KATASHIKI = KATASHIKI;
            this.PROD_SFX = PROD_SFX;
            this.MKT_SFX = MKT_SFX;
            this.GRADE_MARK = GRADE_MARK;
            this.START_LOT = START_LOT;
            this.START_PROD_DATE = START_PROD_DATE;
            this.END_LOT = END_LOT;
            this.END_PROD_DATE = END_PROD_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}
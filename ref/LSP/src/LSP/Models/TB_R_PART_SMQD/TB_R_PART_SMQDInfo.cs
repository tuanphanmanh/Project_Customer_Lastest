using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_PART_SMQD
{ 
    public class TB_R_PART_SMQDInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public long ROW_NO { get; set; }
		public string PART_NO { get; set; }
        public long PART_ID { get; set; }
		public string COLOR_SFX { get; set; }
		public String PART_NAME { get; set; }
		public string BACK_NO { get; set; }
		public string SUPPLIER_CODE { get; set; }
		public DateTime? SMQD_DATETIME { get; set; }
		public string SMQD_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", SMQD_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public int SMQD_QTY { get; set; }
		public string SMQD_TYPE { get; set; }
        public string PIC { get; set; }
        public string RUN_NO { get; set; }
		public String REASON { get; set; }
		public string STATUS { get; set; }
		public string IS_ACTIVE { get; set; }
		public String CREATED_BY { get; set; }
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
		public String UPDATED_BY { get; set; }
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


        public string YEAR { get; set; }
        public string MONTH { get; set; }


		#endregion

		#region "Constructors"
		public TB_R_PART_SMQDInfo() 
		{
			ID = 0;
			PART_NO = string.Empty;
            PART_ID = 0;
			COLOR_SFX = string.Empty;
			PART_NAME = string.Empty;
			BACK_NO = string.Empty;
			SUPPLIER_CODE = string.Empty;
			SMQD_DATETIME = null;
			SMQD_QTY = 0;
			SMQD_TYPE = string.Empty;
            PIC = string.Empty;
            RUN_NO = string.Empty;
			REASON = string.Empty;
			STATUS = string.Empty;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_R_PART_SMQDInfo(long id, string PART_NO, string COLOR_SFX, string PART_NAME, string BACK_NO, string SUPPLIER_CODE,
            DateTime SMQD_DATETIME, int SMQD_QTY, string SMQD_TYPE, string PIC, string RUN_NO, string REASON, string STATUS, string IS_ACTIVE, 
            string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
			this.PART_NO = PART_NO;
			this.COLOR_SFX = COLOR_SFX;
			this.PART_NAME = PART_NAME;
			this.BACK_NO = BACK_NO;
			this.SUPPLIER_CODE = SUPPLIER_CODE;
			this.SMQD_DATETIME = SMQD_DATETIME;
			this.SMQD_QTY = SMQD_QTY;
			this.SMQD_TYPE = SMQD_TYPE;
			this.PIC = PIC;
			this.RUN_NO = RUN_NO;
			this.REASON = REASON;
			this.STATUS = STATUS;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



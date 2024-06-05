using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_PART_RUNDOWN
{ 
    public class TB_R_PART_RUNDOWNInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public long ROW_NO { get; set; }
		public long PART_ID { get; set; }
		public int STOCK_QTY { get; set; }
		public DateTime? STOCK_DATE { get; set; }
		public string STOCK_DATE_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", STOCK_DATE);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
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

        public string SUPPLIER_CODE { get; set; }
        public string PART_NO { get; set; }
        public string COLOR_SFX { get; set; }
        public string PART_NAME { get; set; }
        public int    BOX_SIZE { get; set; }
        public int STD_MIN_STOCK { get; set; }
        public int STD_MAX_STOCK { get; set; }
        public DateTime? STOCK_MONTH_FROM { get; set; }
        public string STOCK_MONTH_FROM_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", STOCK_MONTH_FROM);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public DateTime? STOCK_MONTH_TO { get; set; }
        public string STOCK_MONTH_TO_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", STOCK_MONTH_TO);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public DateTime? STOCK_MONTH { get; set; }
        public int DAY_1 { get; set; }
        public int DAY_2 { get; set; }
        public int DAY_3 { get; set; }
        public int DAY_4 { get; set; }
        public int DAY_5 { get; set; }
        public int DAY_6 { get; set; }
        public int DAY_7 { get; set; }
        public int DAY_8 { get; set; }
        public int DAY_9 { get; set; }
        public int DAY_10 { get; set; }
        public int DAY_11 { get; set; }
        public int DAY_12 { get; set; }
        public int DAY_13 { get; set; }
        public int DAY_14 { get; set; }
        public int DAY_15 { get; set; }
        public int DAY_16 { get; set; }
        public int DAY_17 { get; set; }
        public int DAY_18 { get; set; }
        public int DAY_19 { get; set; }
        public int DAY_20 { get; set; }
        public int DAY_21 { get; set; }
        public int DAY_22 { get; set; }
        public int DAY_23 { get; set; }
        public int DAY_24 { get; set; }
        public int DAY_25 { get; set; }
        public int DAY_26 { get; set; }
        public int DAY_27 { get; set; }
        public int DAY_28 { get; set; }
        public int DAY_29 { get; set; }
        public int DAY_30 { get; set; }
        public int DAY_31 { get; set; }

		#endregion

		#region "Constructors"
		public TB_R_PART_RUNDOWNInfo() 
		{
			ID = 0;
			PART_ID = 0;
			STOCK_QTY = 0;
			STOCK_DATE = null;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_R_PART_RUNDOWNInfo(long id, long PART_ID, int STOCK_QTY, DateTime STOCK_DATE, string IS_ACTIVE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
			this.PART_ID = PART_ID;
			this.STOCK_QTY = STOCK_QTY;
			this.STOCK_DATE = STOCK_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



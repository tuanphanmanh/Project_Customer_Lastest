using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_OR_TIME
{ 
    public class TB_M_SUPPLIER_OR_TIMEInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public int ROW_NO { get; set; }
		public string SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
		public int ORDER_SEQ { get; set; }
        public string ORDER_TYPE { get; set; }
        public int RECEIVING_DAY { get; set; }
        public TimeSpan? ORDER_TIME { get; set; }
        public TimeSpan? RECEIVE_TIME { get; set; }       
        public TimeSpan? KEIHEN_TIME { get; set; }
        public int KEIHEN_DAY { get; set; }
        
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
		public string IS_ACTIVE { get; set; }
		#endregion

		#region "Constructors"
		public TB_M_SUPPLIER_OR_TIMEInfo() 
		{
			ID = 0;
            ROW_NO = 0;
            SUPPLIER_ID = string.Empty;
            SUPPLIER_CODE = string.Empty;
            ORDER_SEQ = 0;
            ORDER_TIME = null;
            RECEIVE_TIME = null;
            KEIHEN_TIME = null;
            ORDER_TYPE = string.Empty;
            RECEIVING_DAY = 0;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
            KEIHEN_DAY = 0;
		}

        public TB_M_SUPPLIER_OR_TIMEInfo(long id, int ROW_NO, string SUPPLIER_ID, int ORDER_SEQ, string ORDER_TYPE,  int RECEIVING_DAY,
                TimeSpan ORDER_TIME, TimeSpan RECEIVE_TIME, TimeSpan KEIHEN_TIME, int KEIHEN_DAY, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE, string IS_ACTIVE)
		{
			this.ID = ID;
            this.ROW_NO = ROW_NO;
			this.SUPPLIER_ID = SUPPLIER_ID;
            this.ORDER_SEQ = ORDER_SEQ;
            this.ORDER_TIME = ORDER_TIME;
            this.RECEIVE_TIME = RECEIVE_TIME;
            this.KEIHEN_TIME = KEIHEN_TIME;
            this.ORDER_TYPE = ORDER_TYPE;
            this.RECEIVING_DAY = RECEIVING_DAY;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
            this.KEIHEN_DAY = KEIHEN_DAY;
		}
		#endregion
    }
}



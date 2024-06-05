using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_DL_TIME
{ 
    public class TB_M_SUPPLIER_DL_TIMEInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public int ROW_NO { get; set; }
		public string SUPPLIER_ID { get; set; }
		public int DELIVERY_SEQ { get; set; }
		public TimeSpan? DELIVERY_TIME { get; set; }
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
		public TB_M_SUPPLIER_DL_TIMEInfo() 
		{
			ID = 0;
            ROW_NO = 0;
            SUPPLIER_ID = string.Empty;
			DELIVERY_SEQ = 0;
			DELIVERY_TIME = null;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
		}
		
		public TB_M_SUPPLIER_DL_TIMEInfo(long id, int ROW_NO, string SUPPLIER_ID, int DELIVERY_SEQ, TimeSpan DELIVERY_TIME, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE, string IS_ACTIVE)
		{
			this.ID = ID;
            this.ROW_NO = ROW_NO;
			this.SUPPLIER_ID = SUPPLIER_ID;
			this.DELIVERY_SEQ = DELIVERY_SEQ;
			this.DELIVERY_TIME = DELIVERY_TIME;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
		}
		#endregion
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_UP_PLAN_H
{ 
    public class TB_R_UP_PLAN_HInfo
	{
		#region "Public Members"
		public long ID { get; set; }
		public string ORDER_NO { get; set; }
		public string LINE { get; set; }
		public string CASE_NO { get; set; }
		public string SUPPLIER_CODE { get; set; }
		public TimeSpan? UNPACKING_TIME { get; set; }
		public DateTime? UNPACKING_DATE { get; set; }
		public string UNPACKING_DATE_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", UNPACKING_DATE);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public int NO_IN_DATE { get; set; }
		public DateTime? WORKING_DATE { get; set; }
		public string WORKING_DATE_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", WORKING_DATE);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public string SHIFT { get; set; }
		public String INCOMP_REASON { get; set; }
		public int UP_STATUS { get; set; }
		public string IS_ACTIVE { get; set; }
		public byte IS_CURRENT { get; set; }
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
		#endregion

		#region "Constructors"
		public TB_R_UP_PLAN_HInfo() 
		{
			ID = 0;
			ORDER_NO = string.Empty;
			LINE = string.Empty;
			CASE_NO = string.Empty;
			SUPPLIER_CODE = string.Empty;
			UNPACKING_TIME = null;
			UNPACKING_DATE = null;
			NO_IN_DATE = 0;
			WORKING_DATE = null;
			SHIFT = string.Empty;
			INCOMP_REASON = string.Empty;
			UP_STATUS = 0;
			IS_ACTIVE = string.Empty;
			IS_CURRENT = 0;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_R_UP_PLAN_HInfo(long id, string ORDER_NO, string LINE, string CASE_NO, string SUPPLIER_CODE, TimeSpan UNPACKING_TIME, DateTime UNPACKING_DATE, int NO_IN_DATE, DateTime WORKING_DATE, string SHIFT, string INCOMP_REASON, int UP_STATUS, string IS_ACTIVE, byte IS_CURRENT, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
			this.ORDER_NO = ORDER_NO;
			this.LINE = LINE;
			this.CASE_NO = CASE_NO;
			this.SUPPLIER_CODE = SUPPLIER_CODE;
			this.UNPACKING_TIME = UNPACKING_TIME;
			this.UNPACKING_DATE = UNPACKING_DATE;
			this.NO_IN_DATE = NO_IN_DATE;
			this.WORKING_DATE = WORKING_DATE;
			this.SHIFT = SHIFT;
			this.INCOMP_REASON = INCOMP_REASON;
			this.UP_STATUS = UP_STATUS;
			this.IS_ACTIVE = IS_ACTIVE;
			this.IS_CURRENT = IS_CURRENT;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



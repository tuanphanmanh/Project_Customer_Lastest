using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_ROUTING
{ 
    public class TB_M_ROUTINGInfo
	{
		#region "Public Members"
		public long ID { get; set; }
		public String SUPPLIER_CODE { get; set; }
		public string DOCK { get; set; }
		public String ADDRESS { get; set; }
		public int ROUTING { get; set; }
		public TimeSpan? PICKING_TIME { get; set; }
		public string TRUCK_NAME { get; set; }
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
		public TB_M_ROUTINGInfo() 
		{
			ID = 0;
			SUPPLIER_CODE = string.Empty;
			DOCK = string.Empty;
			ADDRESS = string.Empty;
			ROUTING = 0;
			PICKING_TIME = null;
			TRUCK_NAME = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
		}
		
		public TB_M_ROUTINGInfo(long id, string SUPPLIER_CODE, string DOCK, string ADDRESS, int ROUTING, TimeSpan PICKING_TIME, string TRUCK_NAME, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE, string IS_ACTIVE)
		{
			this.ID = ID;
			this.SUPPLIER_CODE = SUPPLIER_CODE;
			this.DOCK = DOCK;
			this.ADDRESS = ADDRESS;
			this.ROUTING = ROUTING;
			this.PICKING_TIME = PICKING_TIME;
			this.TRUCK_NAME = TRUCK_NAME;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
		}
		#endregion
    }
}



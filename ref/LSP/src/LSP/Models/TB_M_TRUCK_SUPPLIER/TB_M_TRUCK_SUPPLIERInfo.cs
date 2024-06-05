using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_TRUCK_SUPPLIER
{ 
    public class TB_M_TRUCK_SUPPLIERInfo
	{
		#region "Public Members"
		public int ID { get; set; }
		public int SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
		public string TRUCK_NAME { get; set; }
		public String IS_ACTIVE { get; set; }
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
		public TB_M_TRUCK_SUPPLIERInfo() 
		{
			ID = 0;
			SUPPLIER_ID = 0;
			TRUCK_NAME = string.Empty;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_M_TRUCK_SUPPLIERInfo(int id, int SUPPLIER_ID, string TRUCK_NAME, string IS_ACTIVE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
			this.SUPPLIER_ID = SUPPLIER_ID;
			this.TRUCK_NAME = TRUCK_NAME;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



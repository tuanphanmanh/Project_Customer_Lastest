using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_PIC
{ 
    public class TB_M_SUPPLIER_PICInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public long ROW_NO { get; set; }
		public long SUPPLIER_ID { get; set; }
        public String SUPPLIER_CODE { get; set; }
        public String SUPPLIER_NAME { get; set; }
		public String PIC_NAME { get; set; } 
		public String PIC_TELEPHONE { get; set; }
		public String PIC_EMAIL { get; set; }
        public String IS_MAIN_PIC { get; set; }
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
        public string PIC_USER_ACCOUNT { get; set; }
        public string IS_SEND_EMAIL { get; set; }
        public String PIC_TELEPHONE_2 { get; set; }
        
		#endregion

		#region "Constructors"
		public TB_M_SUPPLIER_PICInfo() 
		{
			ID = 0;
			SUPPLIER_ID = 0;
			PIC_NAME = string.Empty;
			PIC_TELEPHONE = string.Empty;
			PIC_EMAIL = string.Empty;
			IS_MAIN_PIC = null;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
            PIC_USER_ACCOUNT = string.Empty;
            IS_SEND_EMAIL = string.Empty;
            PIC_TELEPHONE_2 = string.Empty;
		}

        public TB_M_SUPPLIER_PICInfo(long id, long SUPPLIER_ID, string PIC_NAME, string PIC_TELEPHONE, string PIC_EMAIL,
            String IS_MAIN_PIC, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE, string IS_ACTIVE)
		{
			this.ID = ID;
			this.SUPPLIER_ID = SUPPLIER_ID;
			this.PIC_NAME = PIC_NAME;
			this.PIC_TELEPHONE = PIC_TELEPHONE;
			this.PIC_EMAIL = PIC_EMAIL;
			this.IS_MAIN_PIC = IS_MAIN_PIC;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
		}
		#endregion
    }
}



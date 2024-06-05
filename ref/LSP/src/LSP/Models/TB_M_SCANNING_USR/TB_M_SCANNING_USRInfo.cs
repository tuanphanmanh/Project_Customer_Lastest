using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SCANNING_USR
{ 
    public class TB_M_SCANNING_USRInfo
	{
		#region "Public Members"
		public int ID { get; set; }
        public int Row_No { get; set; }
		public int USER_ID { get; set; }
		public String USER_NAME { get; set; }
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
		public TB_M_SCANNING_USRInfo() 
		{
			ID = 0;
            Row_No = 0;
			USER_ID = 0;
			USER_NAME = string.Empty;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_M_SCANNING_USRInfo(int ID, int Row_No, int USER_ID, string USER_NAME, string IS_ACTIVE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
            this.Row_No = Row_No;
			this.USER_ID = USER_ID;
			this.USER_NAME = USER_NAME;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



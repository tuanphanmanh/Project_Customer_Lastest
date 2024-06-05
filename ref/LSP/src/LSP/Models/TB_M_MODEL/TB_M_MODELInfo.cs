using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_MODEL
{ 
    public class TB_M_MODELInfo
	{
		#region "Public Members"
		public int ID { get; set; }
        public int Row_No { get; set; }
		public String NAME { get; set; }
		public String ABBREVIATION { get; set; }
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

        public String STAGE { get; set; }
		#endregion

		#region "Constructors"
		public TB_M_MODELInfo() 
		{
			ID = 0;
            Row_No = 0;
			NAME = string.Empty;
			ABBREVIATION = string.Empty;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_M_MODELInfo(int id, int Row_No, string NAME, string ABBREVIATION, string IS_ACTIVE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
            this.Row_No = Row_No;
			this.NAME = NAME;
			this.ABBREVIATION = ABBREVIATION;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



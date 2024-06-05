using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_COLOR
{ 
    public class TB_M_COLORInfo
	{
		#region "Public Members"
		public int ID { get; set; }
        public int ROW_NO { get; set; }
        public string VEHICLE_M_ID { get; set; }

		public string CODE { get; set; }
		public string NAME_EN { get; set; }
		public String NAME_VN { get; set; }
		public string TYPE { get; set; }
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
		public TB_M_COLORInfo() 
		{
			ID = 0;
            ROW_NO = 0;
			CODE = string.Empty;
			NAME_EN = string.Empty;
			NAME_VN = string.Empty;
			TYPE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
		}
		
		public TB_M_COLORInfo(int id, int ROW_NO, string CODE, string NAME_EN, string NAME_VN, string TYPE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE, string IS_ACTIVE)
		{
			this.ID = ID;
            this.ROW_NO = ROW_NO;
			this.CODE = CODE;
			this.NAME_EN = NAME_EN;
			this.NAME_VN = NAME_VN;
			this.TYPE = TYPE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
		}
		#endregion
    }
}



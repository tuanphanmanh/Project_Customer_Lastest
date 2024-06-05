using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_TRUCK_TYPE
{
    public class TB_M_TRUCK_TYPEInfo
    {
        #region "Public Members"
		public int ID { get; set; }
        public int Row_No { get; set; }
		public string NAME { get; set; }
		public decimal WEIGHT { get; set; }
        public decimal COST { get; set; }
        public decimal OT { get; set; }
		public string IS_ACTIVE { get; set; }
		public string CREATED_BY { get; set; }
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
		public TB_M_TRUCK_TYPEInfo() 
		{
			ID = 0;
            Row_No = 0;
			NAME = string.Empty;
			WEIGHT = 0;
            COST = 0;
            OT = 0;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}

        public TB_M_TRUCK_TYPEInfo(int id, int Row_No, string NAME, decimal WEIGHT, decimal COST, decimal OT, string IS_ACTIVE, string CREATED_BY, DateTime? CREATED_DATE, string UPDATED_BY, DateTime? UPDATED_DATE)
		{
			this.ID = ID;
            this.Row_No = Row_No;
			this.NAME = NAME;
			this.WEIGHT = WEIGHT;
            this.COST = COST;
            this.OT = OT;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}
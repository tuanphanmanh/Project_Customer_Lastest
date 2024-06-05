using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PART_IN_OUT
{
    public class TB_R_PART_IN_OUTInfo
    {
        #region "Public Members"
		public int ID { get; set; }
        public int Row_No { get; set; }
		public int PART_ID { get; set; }
		public string IS_IN_OUT { get; set; }
        public int QTY { get; set; }
        public string IN_OUT_BY { get; set; }
        public string IN_ORDER_NO { get; set; }
        public int OUT_PROD_VEHICLE_ID { get; set; }
        public string OUT_PROD_VEHICLE { get; set; }
        public string IS_PROCESS_STOCK { get; set; }
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
		public TB_R_PART_IN_OUTInfo() 
		{
			ID = 0;
            Row_No = 0;
			PART_ID = 0;
			IS_IN_OUT = string.Empty;
            QTY = 0;
            IN_OUT_BY = string.Empty;
            IN_ORDER_NO = string.Empty;
            OUT_PROD_VEHICLE_ID = 0;
            IS_PROCESS_STOCK = string.Empty;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
            OUT_PROD_VEHICLE = string.Empty;
		}

        public TB_R_PART_IN_OUTInfo(int id, int Row_No, int PART_ID, string IS_IN_OUT, int QTY, string IN_OUT_BY, string IN_ORDER_NO, int OUT_PROD_VEHICLE_ID, string IS_PROCESS_STOCK, string IS_ACTIVE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
            this.Row_No = Row_No;
			this.PART_ID = PART_ID;
			this.IS_IN_OUT = IS_IN_OUT;
            this.QTY = QTY;
            this.IN_OUT_BY = IN_OUT_BY;
            this.IN_ORDER_NO = IN_ORDER_NO;
            this.OUT_PROD_VEHICLE_ID = OUT_PROD_VEHICLE_ID;
            this.IS_PROCESS_STOCK = IS_PROCESS_STOCK;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}
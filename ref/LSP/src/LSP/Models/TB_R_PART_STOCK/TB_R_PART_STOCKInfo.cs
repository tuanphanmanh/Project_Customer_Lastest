using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PART_STOCK
{
    public class TB_R_PART_STOCKInfo
    {
        #region "Public Members"
		public int ID { get; set; }
        public int Row_No { get; set; }
		public int PART_ID { get; set; }
		public int STOCK_QTY { get; set; }
		public string IS_ACTIVE { get; set; }
        public bool? IS_ACTIVE_BOL
        {
            get
            {
                return IS_ACTIVE == "Y" ? true : false;
            }
            set
            {
                if (value == true) IS_ACTIVE = "Y";
                else IS_ACTIVE = "N";
            }
        }
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

        public string PART_NO { get; set; }
        public string COLOR_SFX { get; set; }
        public string PART_NAME { get; set; }        
        public string BACK_NO { get; set; }        
        public string SUPPLIER_CODE { get; set; }
        public string LAST_OUT_PROD_VEHICLE { get; set; }
        public string REMARKS { get; set; }
        public DateTime? STOCK_DATE { get; set; }
        public string STOCK_DATE_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", STOCK_DATE);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public DateTime? WORKING_DATE { get; set; }

        public DateTime? STOCK_DATE_FROM { get; set; }
        public DateTime? STOCK_DATE_TO { get; set; }
        public string IS_IN_OUT { get; set; }                
        public string IN_ORDER_NO { get; set; }        
        public string OUT_PROD_VEHICLE { get; set; }
        public string PART_NO_12 { get; set; }
        public int MIN_STOCK { get; set; }
        public int MAX_STOCK { get; set; }
        public string STOCK_WARNING { get; set; }
        public int IN_OUT_QTY { get; set; }
        public string SHOP { get; set; }
        #endregion

        #region "Constructors"
        public TB_R_PART_STOCKInfo() 
		{
			ID = 0;
            Row_No = 0;
			PART_ID = 0;
            STOCK_QTY = 0;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
            LAST_OUT_PROD_VEHICLE = string.Empty;
            REMARKS = string.Empty;
            STOCK_DATE = null;
            WORKING_DATE = null;
            IS_IN_OUT = string.Empty;
            IN_ORDER_NO = string.Empty;
            OUT_PROD_VEHICLE = string.Empty;
            PART_NO_12 = string.Empty;
            MIN_STOCK = 0;
            MAX_STOCK = 0;
            STOCK_WARNING = string.Empty;
            IN_OUT_QTY = 0;
            SHOP = string.Empty;

        }       
		#endregion
    }
}
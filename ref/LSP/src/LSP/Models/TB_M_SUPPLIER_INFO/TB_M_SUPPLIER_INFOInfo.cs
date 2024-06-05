using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_INFO
{ 
    public class TB_M_SUPPLIER_INFOInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public long ROW_NO { get; set; }
		public String SUPPLIER_CODE { get; set; }
		public String SUPPLIER_PLANT_CODE { get; set; }
		public String SUPPLIER_NAME { get; set; }
		public String ADDRESS { get; set; }
		public string DOCK_X { get; set; }
		public String DOCK_X_ADDRESS { get; set; }
		public string DELIVERY_METHOD { get; set; }
		public string DELIVERY_FREQUENCY { get; set; }
		public string CD { get; set; }
		public string ORDER_DATE_TYPE { get; set; }
		public string KEIHEN_TYPE { get; set; }
		public decimal STK_CONCEPT_TMV_MIN { get; set; }
        public decimal STK_CONCEPT_TMV_MAX { get; set; }
        public decimal STK_CONCEPT_SUP_M_MIN { get; set; }
        public decimal STK_CONCEPT_SUP_M_MAX { get; set; }
        public decimal STK_CONCEPT_SUP_P_MIN { get; set; }
        public decimal STK_CONCEPT_SUP_P_MAX { get; set; }
		public int TMV_PRODUCT_PERCENTAGE { get; set; }
		public int PIC_MAIN_ID { get; set; }
		public decimal DELIVERY_LT { get; set; }
		public string PRODUCTION_SHIFT { get; set; }
		public DateTime? TC_FROM { get; set; }
		public string TC_FROM_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", TC_FROM);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public DateTime? TC_TO { get; set; }
		public string TC_TO_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", TC_TO);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
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
        public string SUPPLIER_NAME_EN { get; set; }
        public string ORDER_TYPE { get; set; }

		#endregion

		#region "Constructors"
		public TB_M_SUPPLIER_INFOInfo() 
		{
			ID = 0;
            ROW_NO = 0;
			SUPPLIER_CODE = string.Empty;
			SUPPLIER_PLANT_CODE = string.Empty;
			SUPPLIER_NAME = string.Empty;
			ADDRESS = string.Empty;
			DOCK_X = string.Empty;
			DOCK_X_ADDRESS = string.Empty;
			DELIVERY_METHOD = string.Empty;
			DELIVERY_FREQUENCY = string.Empty;
			CD = string.Empty;
			ORDER_DATE_TYPE = string.Empty;
			KEIHEN_TYPE = string.Empty;
			STK_CONCEPT_TMV_MIN = 0;
			STK_CONCEPT_TMV_MAX = 0;
			STK_CONCEPT_SUP_M_MIN = 0;
			STK_CONCEPT_SUP_M_MAX = 0;
			STK_CONCEPT_SUP_P_MIN = 0;
			STK_CONCEPT_SUP_P_MAX = 0;
			TMV_PRODUCT_PERCENTAGE = 0;
			PIC_MAIN_ID = 0;
			DELIVERY_LT = 0;
			PRODUCTION_SHIFT = string.Empty;
			TC_FROM = null;
			TC_TO = null;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
            SUPPLIER_NAME_EN = string.Empty;
            ORDER_TYPE = string.Empty;

		}				
		#endregion
    }
}



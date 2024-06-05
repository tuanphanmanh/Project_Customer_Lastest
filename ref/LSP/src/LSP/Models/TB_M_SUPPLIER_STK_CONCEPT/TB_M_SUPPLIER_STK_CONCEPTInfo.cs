using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SUPPLIER_STK_CONCEPT
{ 
    public class TB_M_SUPPLIER_STK_CONCEPTInfo
	{
		#region "Public Members"
		public int ID { get; set; }
        public long ROW_NO { get; set; }
		public string SUPPLIER_CODE { get; set; }
		public DateTime? MONTH_STK { get; set; }
		public string MONTH_STK_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", MONTH_STK);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
        public string MONTH_STK_Str_MMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:MM/yyyy}", MONTH_STK);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
		public decimal MIN_STK_1 { get; set; }
		public decimal MIN_STK_2 { get; set; }
		public decimal MIN_STK_3 { get; set; }
		public decimal MIN_STK_4 { get; set; }
		public decimal MIN_STK_5 { get; set; }
		public decimal MIN_STK_6 { get; set; }
		public decimal MIN_STK_7 { get; set; }
		public decimal MIN_STK_8 { get; set; }
		public decimal MIN_STK_9 { get; set; }
		public decimal MIN_STK_10 { get; set; }
		public decimal MIN_STK_11 { get; set; }
		public decimal MIN_STK_12 { get; set; }
		public decimal MIN_STK_13 { get; set; }
		public decimal MIN_STK_14 { get; set; }
		public decimal MIN_STK_15 { get; set; }
		public decimal MAX_STK_1 { get; set; }
		public decimal MAX_STK_2 { get; set; }
		public decimal MAX_STK_3 { get; set; }
		public decimal MAX_STK_4 { get; set; }
		public decimal MAX_STK_5 { get; set; }
		public decimal MIN_STK_CONCEPT { get; set; }
		public decimal MAX_STK_CONCEPT { get; set; }
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

        public string STK_CONCEPT { get; set; }
        public int STK_CONCEPT_FRQ { get; set; }
        public decimal STK_CONCEPT_FRQ_VALUE { get; set; }
		#endregion

		#region "Constructors"
		public TB_M_SUPPLIER_STK_CONCEPTInfo() 
		{
			ID = 0;
            ROW_NO = 0;
			SUPPLIER_CODE = string.Empty;
			MONTH_STK = null;
			MIN_STK_1 = 0;
			MIN_STK_2 = 0;
			MIN_STK_3 = 0;
			MIN_STK_4 = 0;
			MIN_STK_5 = 0;
			MIN_STK_6 = 0;
			MIN_STK_7 = 0;
			MIN_STK_8 = 0;
			MIN_STK_9 = 0;
			MIN_STK_10 = 0;
			MIN_STK_11 = 0;
			MIN_STK_12 = 0;
			MIN_STK_13 = 0;
			MIN_STK_14 = 0;
			MIN_STK_15 = 0;
			MAX_STK_1 = 0;
			MAX_STK_2 = 0;
			MAX_STK_3 = 0;
			MAX_STK_4 = 0;
			MAX_STK_5 = 0;
			MIN_STK_CONCEPT = 0;
			MAX_STK_CONCEPT = 0;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_M_SUPPLIER_STK_CONCEPTInfo(int id, string SUPPLIER_CODE, DateTime MONTH_STK, decimal MIN_STK_1, decimal MIN_STK_2, decimal MIN_STK_3, decimal MIN_STK_4, decimal MIN_STK_5, decimal MIN_STK_6, decimal MIN_STK_7, decimal MIN_STK_8, decimal MIN_STK_9, decimal MIN_STK_10, decimal MIN_STK_11, decimal MIN_STK_12, decimal MIN_STK_13, decimal MIN_STK_14, decimal MIN_STK_15, decimal MAX_STK_1, decimal MAX_STK_2, decimal MAX_STK_3, decimal MAX_STK_4, decimal MAX_STK_5, decimal MIN_STK_CONCEPT, decimal MAX_STK_CONCEPT, string IS_ACTIVE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
			this.SUPPLIER_CODE = SUPPLIER_CODE;
			this.MONTH_STK = MONTH_STK;
			this.MIN_STK_1 = MIN_STK_1;
			this.MIN_STK_2 = MIN_STK_2;
			this.MIN_STK_3 = MIN_STK_3;
			this.MIN_STK_4 = MIN_STK_4;
			this.MIN_STK_5 = MIN_STK_5;
			this.MIN_STK_6 = MIN_STK_6;
			this.MIN_STK_7 = MIN_STK_7;
			this.MIN_STK_8 = MIN_STK_8;
			this.MIN_STK_9 = MIN_STK_9;
			this.MIN_STK_10 = MIN_STK_10;
			this.MIN_STK_11 = MIN_STK_11;
			this.MIN_STK_12 = MIN_STK_12;
			this.MIN_STK_13 = MIN_STK_13;
			this.MIN_STK_14 = MIN_STK_14;
			this.MIN_STK_15 = MIN_STK_15;
			this.MAX_STK_1 = MAX_STK_1;
			this.MAX_STK_2 = MAX_STK_2;
			this.MAX_STK_3 = MAX_STK_3;
			this.MAX_STK_4 = MAX_STK_4;
			this.MAX_STK_5 = MAX_STK_5;
			this.MIN_STK_CONCEPT = MIN_STK_CONCEPT;
			this.MAX_STK_CONCEPT = MAX_STK_CONCEPT;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



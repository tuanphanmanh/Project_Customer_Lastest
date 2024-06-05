using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_DAILY_ORDER_REPORT
{ 
    public class TB_R_DAILY_ORDER_REPORTInfo
	{
		#region "Public Members"		        
        public long ROW_NO { get; set; }
        public DateTime? WORKING_DATE { get; set; }        
        public string SHIFT { get; set; }
        public string SUPPLIER_NAME { get; set; }
		public string SUPPLIER_CODE { get; set; }              				
		public string DOCK_NO { get; set; }
		public string ORDER_NO { get; set; }
        public string CONTENT_NO { get; set; }
        public int    RENBAN_NO { get; set; }
        public int    TRIP_NO { get; set; }
        public string PART_NO { get; set; }
        public string PART_NAME{ get; set; }
        public string COLOR_SFX { get; set; }
        public string ORGANISATION { get; set; }
        public string PC_ADDRESS { get; set; }
		public DateTime? ORDER_DATETIME { get; set; }
		
		public int USAGE_ORDER_QTY { get; set; }
        public int USAGE_ACTUAL_QTY { get; set; }   
   		
		public String CREATED_BY { get; set; }
		public DateTime? CREATED_DATE { get; set; }
		
		public String UPDATED_BY { get; set; }
		public DateTime? UPDATED_DATE { get; set; }
		
		public string IS_ACTIVE { get; set; }
        public string STATUS { get; set; }

        public DateTime? ORDER_MONTH { get; set; }
        public string UNIT { get; set; }
                          
		#endregion

		#region "Constructors"
		public TB_R_DAILY_ORDER_REPORTInfo() 
		{			
            ROW_NO = 0;
            WORKING_DATE = null;
            SHIFT = string.Empty;
			SUPPLIER_NAME = string.Empty;
			SUPPLIER_CODE = string.Empty;
            DOCK_NO = string.Empty;
            ORDER_NO = string.Empty;
            CONTENT_NO = string.Empty;
			RENBAN_NO = 0;
            TRIP_NO = 0;
            PART_NO = string.Empty;
            PART_NAME = string.Empty;
            COLOR_SFX = string.Empty;
            ORGANISATION = string.Empty;
			PC_ADDRESS = string.Empty;						
			ORDER_DATETIME = null;
			
            USAGE_ORDER_QTY = 0;
            USAGE_ACTUAL_QTY = 0;
			
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
            ORDER_MONTH = null;
            UNIT = string.Empty;
		}

        public TB_R_DAILY_ORDER_REPORTInfo(long ROW_NO, DateTime WORKING_DATE, string SHIFT, string SUPPLIER_NAME, string SUPPLIER_CODE,
            string DOCK_NO, string ORDER_NO, string CONTENT_NO, int RENBAN_NO, int TRIP_NO, string PART_NO, string PART_NAME,
            string COLOR_SFX, string ORGANISATION, string PC_ADDRESS, DateTime ORDER_DATETIME, 
            int USAGE_ORDER_QTY, int USAGE_ACTUAL_QTY, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY,DateTime UPDATED_DATE, string IS_ACTIVE)
		{			
            this.ROW_NO = ROW_NO;
            this.WORKING_DATE = WORKING_DATE;
            this.SHIFT = SHIFT;
			this.SUPPLIER_NAME = SUPPLIER_NAME;
			this.SUPPLIER_CODE = SUPPLIER_CODE;
            this.DOCK_NO = DOCK_NO;
            this.ORDER_NO = ORDER_NO;
            this.CONTENT_NO = CONTENT_NO;
			this.RENBAN_NO = RENBAN_NO;
            this.TRIP_NO = TRIP_NO;
            this.PART_NO = PART_NO;
            this.PART_NAME = PART_NAME;
            this.COLOR_SFX = COLOR_SFX;
            this.ORGANISATION = ORGANISATION;
			this.PC_ADDRESS = PC_ADDRESS;
						
			this.ORDER_DATETIME = ORDER_DATETIME;
			
            this.USAGE_ORDER_QTY = USAGE_ORDER_QTY;
            this.USAGE_ACTUAL_QTY = USAGE_ACTUAL_QTY;
			
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;

		}
		#endregion
    }
}



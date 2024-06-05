using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_KANBAN_REPORT
{ 
    public class TB_R_KANBAN_REPORTInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public long ROW_NO { get; set; }
        public string ORDER_ID { get; set; }
        public string ORDER_NO { get; set; }	
        public string CONTENT_LIST_ID { get; set; }
        public string CONTENT_NO { get; set; }
        public string KANBAN_ID { get; set; }
        public string PART_NO { get; set; }	

        public DateTime? WORKING_DATE { get; set; }
        public DateTime? WORKING_DATE_TO { get; set; }
        public string SHIFT { get; set; }
        public string SUPPLIER_NAME { get; set; }
		public string SUPPLIER_CODE { get; set; }
           														        
		public DateTime? EST_ARRIVAL_DATETIME { get; set; }		
		public String CREATED_BY { get; set; }
		public DateTime? CREATED_DATE { get; set; }		
		public String UPDATED_BY { get; set; }
		public DateTime? UPDATED_DATE { get; set; }		
		public string IS_ACTIVE { get; set; }
        public string STATUS { get; set; }              

        public int PLAN_KANBAN_QTY { get; set; }
        public int ACTUAL_KANBAN_QTY { get; set; }
        public int PLAN_KANBAN_GAP_QTY { get; set; }

        public int ACTUAL_PART_QTY { get; set; }
                
        public string RECEIVING_ISSUE { get; set; }
        public string RECEIVING_PIC { get; set; }
        public string RECEIVING_ALARM { get; set; }
        public string RECEIVING_CAUSE { get; set; }
        public string RECEIVING_COUTERMEASURE { get; set; }
        public string RECEIVING_PIC_ACTION { get; set; }
        public string RECEIVING_PIC_RESULT { get; set; }
        public DateTime? RECEIVING_ACT_DATETIME { get; set; }
        public string RECEIVING_STATUS { get; set; }
        public string IS_ALARM_ON { get; set; }
        public string CONFIRM_CODE { get; set; }
        public DateTime? UP_START_ACT { get; set; }
        public DateTime? UP_FINISH_ACT { get; set; }		
        
		#endregion

		#region "Constructors"
		public TB_R_KANBAN_REPORTInfo() 
		{
			ID = 0;
            ROW_NO = 0;
            WORKING_DATE = null;
            WORKING_DATE_TO = null;
            SHIFT = string.Empty;
			SUPPLIER_NAME = string.Empty;
			SUPPLIER_CODE = string.Empty;
            CONTENT_NO = string.Empty;						
			ORDER_NO = string.Empty;
            PART_NO = string.Empty;
								
			EST_ARRIVAL_DATETIME = null;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;

            PLAN_KANBAN_QTY = 0;
            PLAN_KANBAN_GAP_QTY = 0;
            ACTUAL_KANBAN_QTY = 0;
            ACTUAL_PART_QTY = 0;

            RECEIVING_ISSUE = string.Empty;
            RECEIVING_PIC = string.Empty;
            RECEIVING_ALARM = string.Empty;
            RECEIVING_CAUSE = string.Empty;
            RECEIVING_COUTERMEASURE = string.Empty;
            RECEIVING_PIC_ACTION = string.Empty;
            RECEIVING_PIC_RESULT = string.Empty;
            RECEIVING_ACT_DATETIME = null;
            RECEIVING_STATUS = string.Empty;
            IS_ALARM_ON = string.Empty;
            CONFIRM_CODE = string.Empty;
            UP_START_ACT = null;
            UP_FINISH_ACT = null;
		}
        
		#endregion
    }
}



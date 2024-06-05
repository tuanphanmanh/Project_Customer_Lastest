using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_UNLOADING_PLAN
{ 
    public class TB_R_UNLOADING_PLANInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public long ROW_NO { get; set; }
		public string DOCK { get; set; }
		public string TRUCK { get; set; }
		public String SUPPLIERS { get; set; }
        public String SUPPLIERS_RETURN { get; set; }
        public DateTime? WORKING_DATE { get; set; }
        public string WORKING_DATE_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", PLAN_START_UP_DATETIME);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
		public string SHIFT { get; set; }
        public string SHIFT_NOW { get; set; }
		public short SEQUENCE_NO { get; set; }
		public DateTime? PLAN_START_UP_DATETIME { get; set; }
		public string PLAN_START_UP_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy HH:mm:ss}", PLAN_START_UP_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public DateTime? PLAN_FINISH_UP_DATETIME { get; set; }
		public string PLAN_FINISH_UP_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", PLAN_FINISH_UP_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public DateTime? ACTUAL_START_UP_DATETIME { get; set; }
		public string ACTUAL_START_UP_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", ACTUAL_START_UP_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public DateTime? ACTUAL_FINISH_UP_DATETIME { get; set; }
		public string ACTUAL_FINISH_UP_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", ACTUAL_FINISH_UP_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public DateTime? REVISED_PLAN_START_UP_DATETIME { get; set; }
		public string REVISED_PLAN_START_UP_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", REVISED_PLAN_START_UP_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public DateTime? REVISED_PLAN_FINISH_UP_DATETIME { get; set; }
		public string REVISED_PLAN_FINISH_UP_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", REVISED_PLAN_FINISH_UP_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public int ACTUAL_START_UP_DELAY { get; set; }
		public int ACTUAL_FINISH_UP_DELAY { get; set; }
		public string STATUS { get; set; }
        public string STATUS_DESC { get; set; }
		public String ISSUES { get; set; }
		public String CAUSE { get; set; }
		public String COUTERMEASURE { get; set; }
		public String PIC_RECORDER { get; set; }
		public String PIC_ACTION { get; set; }
		public DateTime? ACTION_DUEDATE { get; set; }
		public string ACTION_DUEDATE_Str_DDMMYYYY
		{
			get 
			{
				try
				{
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", ACTION_DUEDATE);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public short RESULT { get; set; }
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
        public string STATUS_DESC_2 { get; set; }
        public DateTime? WORKING_DATE_FROM { get; set; }

        public string IS_WARNING_DELAY { get; set; }

		#endregion

		#region "Constructors"
		public TB_R_UNLOADING_PLANInfo() 
		{
			ID = 0;
            ROW_NO = 0;
			DOCK = string.Empty;
			TRUCK = string.Empty;
			SUPPLIERS = string.Empty;
            SUPPLIERS_RETURN = string.Empty;
            WORKING_DATE = null;
			SHIFT = string.Empty;
			SEQUENCE_NO = 0;
			PLAN_START_UP_DATETIME = null;
			PLAN_FINISH_UP_DATETIME = null;
			ACTUAL_START_UP_DATETIME = null;
			ACTUAL_FINISH_UP_DATETIME = null;
			REVISED_PLAN_START_UP_DATETIME = null;
			REVISED_PLAN_FINISH_UP_DATETIME = null;
			ACTUAL_START_UP_DELAY = 0;
			ACTUAL_FINISH_UP_DELAY = 0;
			STATUS = string.Empty;
            STATUS_DESC = string.Empty;
			ISSUES = string.Empty;
			CAUSE = string.Empty;
			COUTERMEASURE = string.Empty;
			PIC_RECORDER = string.Empty;
			PIC_ACTION = string.Empty;
			ACTION_DUEDATE = null;
			RESULT = 0;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
            STATUS_DESC_2 = string.Empty; // For ETA
            WORKING_DATE_FROM = null;
            IS_WARNING_DELAY = string.Empty;
		}
		
		public TB_R_UNLOADING_PLANInfo(long id, long ROW_NO, string DOCK, string TRUCK, string SUPPLIERS, DateTime WORKING_DATE, string SHIFT, short SEQUENCE_NO, DateTime PLAN_START_UP_DATETIME, DateTime PLAN_FINISH_UP_DATETIME, DateTime ACTUAL_START_UP_DATETIME, DateTime ACTUAL_FINISH_UP_DATETIME, DateTime REVISED_PLAN_START_UP_DATETIME, DateTime REVISED_PLAN_FINISH_UP_DATETIME, int ACTUAL_START_UP_DELAY, int ACTUAL_FINISH_UP_DELAY, string STATUS, string ISSUES, string CAUSE, string COUTERMEASURE, string PIC_RECORDER, string PIC_ACTION, DateTime ACTION_DUEDATE, short RESULT, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE, string IS_ACTIVE)
		{
			this.ID = ID;
            this.ROW_NO = ROW_NO;
			this.DOCK = DOCK;
			this.TRUCK = TRUCK;
			this.SUPPLIERS = SUPPLIERS;
			this.WORKING_DATE = WORKING_DATE;
			this.SHIFT = SHIFT;
			this.SEQUENCE_NO = SEQUENCE_NO;
			this.PLAN_START_UP_DATETIME = PLAN_START_UP_DATETIME;
			this.PLAN_FINISH_UP_DATETIME = PLAN_FINISH_UP_DATETIME;
			this.ACTUAL_START_UP_DATETIME = ACTUAL_START_UP_DATETIME;
			this.ACTUAL_FINISH_UP_DATETIME = ACTUAL_FINISH_UP_DATETIME;
			this.REVISED_PLAN_START_UP_DATETIME = REVISED_PLAN_START_UP_DATETIME;
			this.REVISED_PLAN_FINISH_UP_DATETIME = REVISED_PLAN_FINISH_UP_DATETIME;
			this.ACTUAL_START_UP_DELAY = ACTUAL_START_UP_DELAY;
			this.ACTUAL_FINISH_UP_DELAY = ACTUAL_FINISH_UP_DELAY;
			this.STATUS = STATUS;
			this.ISSUES = ISSUES;
			this.CAUSE = CAUSE;
			this.COUTERMEASURE = COUTERMEASURE;
			this.PIC_RECORDER = PIC_RECORDER;
			this.PIC_ACTION = PIC_ACTION;
			this.ACTION_DUEDATE = ACTION_DUEDATE;
			this.RESULT = RESULT;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
		}
		#endregion
    }
}



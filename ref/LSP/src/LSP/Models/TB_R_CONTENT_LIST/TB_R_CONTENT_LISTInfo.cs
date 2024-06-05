using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_CONTENT_LIST
{ 
    public class TB_R_CONTENT_LISTInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public string ORDER_ID { get; set; }
        public long ROW_NO { get; set; }
        public DateTime? WORKING_DATE { get; set; }
        public string WORKING_DATE_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", WORKING_DATE);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public string SHIFT { get; set; }
        public string SUPPLIER_NAME { get; set; }
		public string SUPPLIER_CODE { get; set; }
        public string CONTENT_NO { get; set; }        
		public int RENBAN_NO { get; set; }
		public string PC_ADDRESS { get; set; }
		public string DOCK_NO { get; set; }
		public string ORDER_NO { get; set; }
		public DateTime? ORDER_DATETIME { get; set; }
		public string ORDER_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", ORDER_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
        public string ORDER_DATETIME_Str_DDMMYYYY_HHMM
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy hh:mm}", ORDER_DATETIME);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

		public int TRIP_NO { get; set; }
		public int PALLET_BOX_QTY { get; set; }
        public int SCAN_QTY { get; set; } 
		public DateTime? EST_PACKING_DATETIME { get; set; }
		public string EST_PACKING_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", EST_PACKING_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
        public string EST_PACKING_DATETIME_Str_DDMMYYYY_HHMM
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy hh:mm}", EST_PACKING_DATETIME);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
		public DateTime? EST_ARRIVAL_DATETIME { get; set; }
		public string EST_ARRIVAL_DATETIME_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", EST_ARRIVAL_DATETIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
        public string EST_ARRIVAL_DATETIME_Str_DDMMYYYY_HHMM
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy hh:mm}", EST_ARRIVAL_DATETIME);
                }
                catch (Exception ex)
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
        public string STATUS { get; set; }

        public int PALLET_SIZE { get; set; }
        public string IS_PALLET_ONLY { get; set; }

        public string PACKAGING_TYPE { get; set; }

        public int PLAN_PALLET_QTY { get; set; }
        public int ACTUAL_PALLET_QTY { get; set; }
        public string ETA { get; set; }

        //For Report
        public int PLAN_PALLET_GAP_QTY { get; set; }
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
        public string TRUCK_NAME { get; set; }
        public string TRUCK_QTY_REMARK { get; set; }
        public string MODULE_CD { get; set; }
        public int MODULE_RUN_NO { get; set; }
        public string MODULE_NO { get; set; }

        public int PLAN_PALLET_QTY_NOT_SCAN { get; set; }

        public string IS_FUTURE { get; set; }
        public bool? IS_FUTURE_BOL
        {
            get
            {
                return IS_FUTURE == "Y" ? true : false;
            }
            set
            {
                if (value == true) IS_FUTURE = "Y";
                else IS_FUTURE = "N";
            }
        }
		#endregion

		#region "Constructors"
		public TB_R_CONTENT_LISTInfo() 
		{
			ID = 0;
            ROW_NO = 0;
            WORKING_DATE = null;
            SHIFT = string.Empty;
			SUPPLIER_NAME = string.Empty;
			SUPPLIER_CODE = string.Empty;
            CONTENT_NO = string.Empty;
			RENBAN_NO = 0;
			PC_ADDRESS = string.Empty;
			DOCK_NO = string.Empty;
			ORDER_NO = string.Empty;
			ORDER_DATETIME = null;
			TRIP_NO = 0;
			PALLET_BOX_QTY = 0;
			EST_PACKING_DATETIME = null;
			EST_ARRIVAL_DATETIME = null;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
            PALLET_SIZE = 0;
            IS_PALLET_ONLY = string.Empty;
            PACKAGING_TYPE = string.Empty;

            PLAN_PALLET_GAP_QTY = 0;
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
            TRUCK_NAME = string.Empty;
            TRUCK_QTY_REMARK = string.Empty;
            MODULE_CD = string.Empty;
            MODULE_RUN_NO = 0;
            MODULE_NO = string.Empty;
            PLAN_PALLET_QTY_NOT_SCAN = 0;
            IS_FUTURE = string.Empty;
            
		}

        public TB_R_CONTENT_LISTInfo(long id, long ROW_NO, DateTime WORKING_DATE, string SHIFT, string SUPPLIER_NAME, string SUPPLIER_CODE, 
            string CONTENT_NO,int RENBAN_NO, string PC_ADDRESS, string DOCK_NO, string ORDER_NO, DateTime ORDER_DATETIME, int TRIP_NO, 
            int PALLET_BOX_QTY, DateTime EST_PACKING_DATETIME, DateTime EST_ARRIVAL_DATETIME, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY,
            DateTime UPDATED_DATE, string IS_ACTIVE, int PALLET_SIZE, string IS_PALLET_ONLY, string PACKAGING_TYPE)
		{
			this.ID = ID;
            this.ROW_NO = ROW_NO;
            this.WORKING_DATE = WORKING_DATE;
            this.SHIFT = SHIFT;
			this.SUPPLIER_NAME = SUPPLIER_NAME;
			this.SUPPLIER_CODE = SUPPLIER_CODE;
            this.CONTENT_NO = CONTENT_NO;
			this.RENBAN_NO = RENBAN_NO;
			this.PC_ADDRESS = PC_ADDRESS;
			this.DOCK_NO = DOCK_NO;
			this.ORDER_NO = ORDER_NO;
			this.ORDER_DATETIME = ORDER_DATETIME;
			this.TRIP_NO = TRIP_NO;
			this.PALLET_BOX_QTY = PALLET_BOX_QTY;
			this.EST_PACKING_DATETIME = EST_PACKING_DATETIME;
			this.EST_ARRIVAL_DATETIME = EST_ARRIVAL_DATETIME;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
            this.PALLET_SIZE = PALLET_SIZE;
            this.IS_PALLET_ONLY = IS_PALLET_ONLY;
            this.PACKAGING_TYPE = PACKAGING_TYPE;

		}
		#endregion
    }
}



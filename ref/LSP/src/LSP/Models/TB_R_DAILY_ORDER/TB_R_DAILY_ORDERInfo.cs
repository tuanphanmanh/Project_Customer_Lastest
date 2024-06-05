using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_DAILY_ORDER
{ 
    public class TB_R_DAILY_ORDERInfo
	{
		#region "Public Members"
		public long ID { get; set; }
		public DateTime? WORKING_DATE { get; set; }
		public string WORKING_DATE_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", WORKING_DATE);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public string SHIFT { get; set; }
		public String SUPPLIER_NAME { get; set; }
		public string SUPPLIER_CODE { get; set; }
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
        public string ORDER_DATETIME_Str_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy hh:mm:ss}", ORDER_DATETIME);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

		public int TRIP_NO { get; set; }
		public string TRUCK_NO { get; set; }
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
        public string EST_ARRIVAL_DATETIME_Str_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy hh:mm:ss}", EST_ARRIVAL_DATETIME);
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
        public string GENERATE_BY { get; set; }

        public int PLAN_PALLET_QTY { get; set; }
        public int ACTUAL_PALLET_QTY { get; set; }
        public string IS_BY_RECEIVING_DAY { get; set; }
        public bool? IS_BY_RECEIVING_DAY_BOL
        {
            get
            {
                return IS_BY_RECEIVING_DAY == "Y" ? true : false;
            }
            set
            {
                if (value == true) IS_BY_RECEIVING_DAY = "Y";
                else IS_BY_RECEIVING_DAY = "N";
            }
        }

        public string USER_NAME { get; set; }
		public string DOCK_NO { get; set; }


		#endregion

		#region "Constructors"
		public TB_R_DAILY_ORDERInfo() 
		{
			ID = 0;
			WORKING_DATE = null;
			SHIFT = string.Empty;
			SUPPLIER_NAME = string.Empty;
			SUPPLIER_CODE = string.Empty;
			ORDER_NO = string.Empty;
			ORDER_DATETIME = null;
			TRIP_NO = 0;
			TRUCK_NO = string.Empty;
			EST_ARRIVAL_DATETIME = null;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
			STATUS = string.Empty;
            PLAN_PALLET_QTY = 0;
            ACTUAL_PALLET_QTY = 0;
            GENERATE_BY = string.Empty;
            IS_BY_RECEIVING_DAY = string.Empty;
            USER_NAME = string.Empty;
			DOCK_NO = string.Empty;
		}
		
		public TB_R_DAILY_ORDERInfo(long id, DateTime WORKING_DATE, string SHIFT, string SUPPLIER_NAME, string SUPPLIER_CODE, string ORDER_NO, 
            DateTime ORDER_DATETIME, int TRIP_NO, string TRUCK_NO, DateTime EST_ARRIVAL_DATETIME, string CREATED_BY, DateTime CREATED_DATE,
            string UPDATED_BY, DateTime UPDATED_DATE, string IS_ACTIVE, string STATUS,  string GENERATE_BY, int PLAN_PALLET_QTY, int ACTUAL_PALLET_QTY)
		{
			this.ID = ID;
			this.WORKING_DATE = WORKING_DATE;
			this.SHIFT = SHIFT;
			this.SUPPLIER_NAME = SUPPLIER_NAME;
			this.SUPPLIER_CODE = SUPPLIER_CODE;
			this.ORDER_NO = ORDER_NO;
			this.ORDER_DATETIME = ORDER_DATETIME;
			this.TRIP_NO = TRIP_NO;
			this.TRUCK_NO = TRUCK_NO;
			this.EST_ARRIVAL_DATETIME = EST_ARRIVAL_DATETIME;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
			this.STATUS = STATUS;
            this.GENERATE_BY = GENERATE_BY;
            this.PLAN_PALLET_QTY = PLAN_PALLET_QTY;
            this.ACTUAL_PALLET_QTY = ACTUAL_PALLET_QTY;
		}
		#endregion
    }
}



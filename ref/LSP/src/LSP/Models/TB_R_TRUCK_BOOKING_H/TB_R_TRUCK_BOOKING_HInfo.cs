using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_TRUCK_BOOKING_H
{
    public class TB_R_TRUCK_BOOKING_HInfo
    {
        #region "Public Members"
		public long ID { get; set; }
        public long Row_No { get; set; }

        public long UNLOADING_PLAN_H_ID { get; set; }
		public DateTime? ETA_REQUEST { get; set; }
        public DateTime? ETD_REQUEST { get; set; }
        public string ETA_REQUEST_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", ETA_REQUEST);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
        public string TRANSPORTER_ABBR { get; set; }
        public string TRUCK { get; set; }
        public string TRUCK_TYPE { get; set; }
        public string SUPPLIERS { get; set; }
        public int TRIP_NO { get; set; }
        public string PATH { get; set; }
        public string DOCK { get; set; } 
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
		public TB_R_TRUCK_BOOKING_HInfo() 
		{
			ID = 0;
            Row_No = 0;
            UNLOADING_PLAN_H_ID = 0;
			ETA_REQUEST = null;
            ETD_REQUEST = null;
            PATH = string.Empty;
			TRANSPORTER_ABBR = string.Empty;
            TRUCK = string.Empty;
            TRUCK_TYPE = string.Empty;
            DOCK = string.Empty;
            SUPPLIERS = string.Empty;
            TRIP_NO = 0;
            IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}        
		#endregion
    }
}
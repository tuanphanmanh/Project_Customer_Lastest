using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_UNLOADING_PLAN_H
{ 
    public class TB_R_UNLOADING_PLAN_HInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public long ROW_NO { get; set; }
		public string DOCK { get; set; }
		public string TRUCK { get; set; }
		public String SUPPLIERS { get; set; }
        public String SUPPLIERS_RETURN { get; set; }
		public DateTime? FROM_DATE { get; set; }
		public string FROM_DATE_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", FROM_DATE);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
        public DateTime? PLAN_START_UL_TIME { get; set; }
        public string PLAN_START_UL_TIME_Str_HHMMSS
		{
			get 
			{
				try
				{
                    return string.Format("{0:HH:mm:ss}", PLAN_START_UL_TIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
        public DateTime? PLAN_FINISH_UL_TIME { get; set; }
        public string PLAN_FINISH_UL_TIME_Str_HHMMSS
		{
			get 
			{
				try
				{
                    return string.Format("{0:HH:mm:ss}", PLAN_FINISH_UL_TIME);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
        public int TRIP_NO { get; set; }
		public string ANDON_NO { get; set; }
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
		public string IS_EPE { get; set; }
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
		#endregion

		#region "Constructors"
		public TB_R_UNLOADING_PLAN_HInfo() 
		{
			ID = 0;
			DOCK = string.Empty;
			TRUCK = string.Empty;
			SUPPLIERS = string.Empty;
            SUPPLIERS_RETURN = string.Empty;
			FROM_DATE = null;
            PLAN_START_UL_TIME = null;
            PLAN_FINISH_UL_TIME = null;
			ANDON_NO = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
            TRIP_NO = 0;
            ROW_NO = 0;
			IS_EPE = string.Empty;
		}
		
		public TB_R_UNLOADING_PLAN_HInfo(long id, long ROW_NO, int TRIP_NO, string DOCK, string TRUCK, string SUPPLIERS, DateTime FROM_DATE,
            DateTime PLAN_START_UL_TIME, DateTime PLAN_FINISH_UL_TIME, string ANDON_NO, string CREATED_BY, DateTime CREATED_DATE, 
            string UPDATED_BY, DateTime UPDATED_DATE, string IS_ACTIVE)
		{
			this.ID = ID;
            this.ROW_NO = ROW_NO;
			this.DOCK = DOCK;
			this.TRUCK = TRUCK;
			this.SUPPLIERS = SUPPLIERS;
			this.FROM_DATE = FROM_DATE;
            this.PLAN_START_UL_TIME = PLAN_START_UL_TIME;
            this.PLAN_FINISH_UL_TIME = PLAN_FINISH_UL_TIME;
			this.ANDON_NO = ANDON_NO;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
            this.TRIP_NO = TRIP_NO;
		}
		#endregion
    }
}



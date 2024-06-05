using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_KANBAN
{ 
    public class TB_R_KANBANInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public long ROW_NO { get; set; }
		public string CONTENT_LIST_ID { get; set; }
        public string CONTENT_NO { get; set; }
		public string BACK_NO { get; set; }
		public string PART_NO { get; set; }
        public string MODEL { get; set; }
		public string COLOR_SFX { get; set; }
		public String PART_NAME { get; set; }
		public int BOX_SIZE { get; set; }
        public int ACTUAL_BOX_SIZE { get; set; }        
		public int BOX_QTY { get; set; }
        public int ACTUAL_BOX_QTY { get; set; }
		public string PC_ADDRESS { get; set; }
        public string OVER { get; set; }
		public string WH_SPS_PICKING { get; set; }
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
        public int RENBAN_NO { get; set; }
        public int NO_IN_RENBAN { get; set; }

        public int PALLET_BOX_QTY { get; set; }
        public int BOX_QTY_2 { get; set; } //using for count real box
		public int BOX_QTY_3 { get; set; } //using for count real box
		public string COLOR { get; set; }
        public string PACKAGING_TYPE { get; set; }
        public string PCS { get; set; }
        public string IS_ENABLE_EDIT { get; set; }
        public string IS_ALARM_ON { get; set; }
        public string REMARK { get; set; }
        public DateTime? UP_EST_DATETIME { get; set; }
        public string UP_EST_DATETIME_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:HH:mm:ss}", UP_EST_DATETIME);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public string UNPACK_STATUS { get; set; }
        public int PLAN_BOX_QTY_NOT_SCAN { get; set; }
		#endregion

		#region "Constructors"
		public TB_R_KANBANInfo() 
		{
			ID = 0;
            CONTENT_LIST_ID = string.Empty;
			BACK_NO = string.Empty;
			PART_NO = string.Empty;
			COLOR_SFX = string.Empty;
			PART_NAME = string.Empty;
			BOX_SIZE = 0;
            ACTUAL_BOX_SIZE = 0;
			BOX_QTY = 0;
			BOX_QTY_3 = 0;
			PC_ADDRESS = string.Empty;
			WH_SPS_PICKING = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
            ACTUAL_BOX_QTY = 0;
            STATUS = string.Empty;
            RENBAN_NO = 0;
            NO_IN_RENBAN = 0;
            PCS = string.Empty;
            IS_ENABLE_EDIT = string.Empty;
            IS_ALARM_ON = string.Empty;
            REMARK = string.Empty;
            UP_EST_DATETIME = null;
            UNPACK_STATUS = string.Empty;
            PLAN_BOX_QTY_NOT_SCAN = 0;
		}

        public TB_R_KANBANInfo(long id, string CONTENT_LIST_ID, string BACK_NO, string PART_NO, string COLOR_SFX, string PART_NAME, int BOX_SIZE, int BOX_QTY,
            string PC_ADDRESS, string WH_SPS_PICKING, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE, string IS_ACTIVE,
            int ACTUAL_BOX_QTY, string STATUS, int RENBAN_NO, int NO_IN_RENBAN, string IS_ENABLE_EDIT)
		{
			this.ID = ID;
			this.CONTENT_LIST_ID = CONTENT_LIST_ID;
			this.BACK_NO = BACK_NO;
			this.PART_NO = PART_NO;
			this.COLOR_SFX = COLOR_SFX;
			this.PART_NAME = PART_NAME;
			this.BOX_SIZE = BOX_SIZE;
			this.BOX_QTY = BOX_QTY;
			this.PC_ADDRESS = PC_ADDRESS;
			this.WH_SPS_PICKING = WH_SPS_PICKING;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
            this.ACTUAL_BOX_QTY = ACTUAL_BOX_QTY;
            this.STATUS = STATUS;
            this.RENBAN_NO = RENBAN_NO;
            this.NO_IN_RENBAN = NO_IN_RENBAN;
            this.IS_ENABLE_EDIT = IS_ENABLE_EDIT;
		}
		#endregion
    }
}



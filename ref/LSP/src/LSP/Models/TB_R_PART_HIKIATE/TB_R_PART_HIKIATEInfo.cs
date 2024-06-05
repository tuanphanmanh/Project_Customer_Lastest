using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_PART_HIKIATE
{ 
    public class TB_R_PART_HIKIATEInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public long ROW_NO { get; set; }
		public string CFC { get; set; }
		public string PROD_SFX { get; set; }
		public string PART_NO { get; set; }
		public string COLOR_SFX { get; set; }
        public string PART_NAME { get; set; }
		public int QTY_PER_VEHICLE { get; set; }
		public string BACK_NO { get; set; }
		public string PARTS_MACHING_KEY { get; set; }
		public string SUPPLIER_CODE { get; set; }
		public string SHOP { get; set; }
		public string DOCK { get; set; }
		public string ORGANISATION { get; set; }
		public int RECEIVING_TIME { get; set; }
		public DateTime? PLANT_TC_FROM { get; set; }
        public string PLANT_TC_FROM_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", PLANT_TC_FROM);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public DateTime? PLANT_TC_TO { get; set; }
        public string PLANT_TC_TO_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", PLANT_TC_TO);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

		public string START_LOT { get; set; }
		public string END_LOT { get; set; }
		public int BOX_SIZE { get; set; }
		public int PACKING_MIX { get; set; }
		public decimal BOX_WEIGHT { get; set; }
		public int BOX_W { get; set; }
		public int BOX_H { get; set; }
		public int BOX_L { get; set; }
		public decimal PALLET_WEIGHT { get; set; }
		public int QTY_BOX_PER_PALLET { get; set; }
		public int PALLET_W { get; set; }
		public int PALLET_H { get; set; }
		public int PALLET_L { get; set; }
		public string UNIT { get; set; }
		public decimal COST { get; set; }
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
		public string IS_ACTIVE { get; set; }
        public string DELIVERY_PROCESS { get; set; }
        public string PACKAGING_TYPE { get; set; }
        public string COLOR { get; set; }

        public string MODULE_CD { get; set; }
		public string REMARKS { get; set; }
		

		#endregion

		#region "Constructors"
		public TB_R_PART_HIKIATEInfo() 
		{
			ID = 0;
            ROW_NO = 0;
			CFC = string.Empty;
			PROD_SFX = string.Empty;
			PART_NO = string.Empty;
			COLOR_SFX = string.Empty;
			PART_NAME = string.Empty;
            QTY_PER_VEHICLE = 0;
			BACK_NO = string.Empty;
			PARTS_MACHING_KEY = string.Empty;
			SUPPLIER_CODE = string.Empty;
			SHOP = string.Empty;
			DOCK = string.Empty;
			ORGANISATION = string.Empty;
			RECEIVING_TIME = 0;
            PLANT_TC_FROM = null;
            PLANT_TC_TO = null;
			START_LOT = string.Empty;
			END_LOT = string.Empty;
			BOX_SIZE = 0;
			PACKING_MIX = 0;
			BOX_WEIGHT = 0;
			BOX_W = 0;
			BOX_H = 0;
			BOX_L = 0;
			PALLET_WEIGHT = 0;
			QTY_BOX_PER_PALLET = 0;
			PALLET_W = 0;
			PALLET_H = 0;
			PALLET_L = 0;
			UNIT = string.Empty;
			COST = 0;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			IS_ACTIVE = string.Empty;
            DELIVERY_PROCESS = string.Empty;
            PACKAGING_TYPE = string.Empty;

            MODULE_CD = string.Empty;
			REMARKS = string.Empty;
		}

        public TB_R_PART_HIKIATEInfo(long id, long ROW_NO, string CFC, string PROD_SFX, string PART_NO, string COLOR_SFX, string PART_NAME, int QTY_PER_VEHICLE, 
            string BACK_NO, string PARTS_MACHING_KEY, string SUPPLIER_CODE, string SHOP, string DOCK, string ORGANISATION, int RECEIVING_TIME,
            DateTime PLANT_TC_FROM, DateTime PLANT_TC_TO, string START_LOT, string END_LOT, int BOX_SIZE, int PACKING_MIX, decimal BOX_WEIGHT, 
            int BOX_W, int BOX_H, int BOX_L, decimal PALLET_WEIGHT, int QTY_BOX_PER_PALLET, int PALLET_W, int PALLET_H, int PALLET_L, string UNIT, decimal COST,
            string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE,
            string IS_ACTIVE, string DELIVERY_PROCESS, string PACKAGING_TYPE)
		{
			this.ID = ID;
            this.ROW_NO = ROW_NO;
			this.CFC = CFC;
			this.PROD_SFX = PROD_SFX;
			this.PART_NO = PART_NO;
			this.COLOR_SFX = COLOR_SFX;
			this.PART_NAME = PART_NAME;
			this.QTY_PER_VEHICLE = QTY_PER_VEHICLE;
			this.BACK_NO = BACK_NO;
			this.PARTS_MACHING_KEY = PARTS_MACHING_KEY;
			this.SUPPLIER_CODE = SUPPLIER_CODE;
			this.SHOP = SHOP;
			this.DOCK = DOCK;
			this.ORGANISATION = ORGANISATION;
			this.RECEIVING_TIME = RECEIVING_TIME;
			this.PLANT_TC_FROM = PLANT_TC_FROM;
			this.PLANT_TC_TO = PLANT_TC_TO;
			this.START_LOT = START_LOT;
			this.END_LOT = END_LOT;
			this.BOX_SIZE = BOX_SIZE;
			this.PACKING_MIX = PACKING_MIX;
			this.BOX_WEIGHT = BOX_WEIGHT;
			this.BOX_W = BOX_W;
			this.BOX_H = BOX_H;
			this.BOX_L = BOX_L;
			this.PALLET_WEIGHT = PALLET_WEIGHT;
			this.QTY_BOX_PER_PALLET = QTY_BOX_PER_PALLET;
			this.PALLET_W = PALLET_W;
			this.PALLET_H = PALLET_H;
			this.PALLET_L = PALLET_L;
			this.UNIT = UNIT;
			this.COST = COST;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
			this.IS_ACTIVE = IS_ACTIVE;
            this.DELIVERY_PROCESS = DELIVERY_PROCESS;
            this.PACKAGING_TYPE = PACKAGING_TYPE;
		}
		#endregion
    }
}



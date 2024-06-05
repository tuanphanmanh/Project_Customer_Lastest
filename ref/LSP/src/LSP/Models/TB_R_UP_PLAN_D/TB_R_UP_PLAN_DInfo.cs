using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_UP_PLAN_D
{ 
    public class TB_R_UP_PLAN_DInfo
	{
		#region "Public Members"
		public long ID { get; set; }
		public string UP_PLAN_H_ID { get; set; }
		public string LINE { get; set; }
		public int NO { get; set; }
		public string BACK_NO { get; set; }
		public string CASE_NO { get; set; }
		public string SUPPLIER_NO { get; set; }
		public string MODEL { get; set; }
		public string PART_NO { get; set; }
		public String PART_NAME { get; set; }
		public String PC_ADDRESS { get; set; }
		public int QTY { get; set; }
		public int BOX_SIZE { get; set; }
		public int QTY_BOX { get; set; }
		public int QTY_ACT { get; set; }
		public String PXP_LOCATION { get; set; }
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
		public string UP_STATUS { get; set; }
		public String INCOMP_REASON { get; set; }
		public string IS_ACTIVE { get; set; }
		public byte IS_OVER { get; set; }
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
		#endregion

		#region "Constructors"
		public TB_R_UP_PLAN_DInfo() 
		{
			ID = 0;
            UP_PLAN_H_ID = string.Empty;
			LINE = string.Empty;
			NO = 0;
			BACK_NO = string.Empty;
			CASE_NO = string.Empty;
			SUPPLIER_NO = string.Empty;
			MODEL = string.Empty;
			PART_NO = string.Empty;
			PART_NAME = string.Empty;
			PC_ADDRESS = string.Empty;
			QTY = 0;
			BOX_SIZE = 0;
			QTY_BOX = 0;
			QTY_ACT = 0;
			PXP_LOCATION = string.Empty;
			WORKING_DATE = null;
			SHIFT = string.Empty;
			UP_STATUS = string.Empty;
			INCOMP_REASON = string.Empty;
			IS_ACTIVE = string.Empty;
			IS_OVER = 0;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_R_UP_PLAN_DInfo(long id, string UP_PLAN_H_ID, string LINE, int NO, string BACK_NO, string CASE_NO, string SUPPLIER_NO, string MODEL, string PART_NO, string PART_NAME, string PC_ADDRESS, int QTY, int BOX_SIZE, int QTY_BOX, int QTY_ACT, string PXP_LOCATION, DateTime WORKING_DATE, string SHIFT, string UP_STATUS, string INCOMP_REASON, string IS_ACTIVE, byte IS_OVER, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
			this.UP_PLAN_H_ID = UP_PLAN_H_ID;
			this.LINE = LINE;
			this.NO = NO;
			this.BACK_NO = BACK_NO;
			this.CASE_NO = CASE_NO;
			this.SUPPLIER_NO = SUPPLIER_NO;
			this.MODEL = MODEL;
			this.PART_NO = PART_NO;
			this.PART_NAME = PART_NAME;
			this.PC_ADDRESS = PC_ADDRESS;
			this.QTY = QTY;
			this.BOX_SIZE = BOX_SIZE;
			this.QTY_BOX = QTY_BOX;
			this.QTY_ACT = QTY_ACT;
			this.PXP_LOCATION = PXP_LOCATION;
			this.WORKING_DATE = WORKING_DATE;
			this.SHIFT = SHIFT;
			this.UP_STATUS = UP_STATUS;
			this.INCOMP_REASON = INCOMP_REASON;
			this.IS_ACTIVE = IS_ACTIVE;
			this.IS_OVER = IS_OVER;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



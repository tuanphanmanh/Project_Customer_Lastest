using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_PART_HIKIATE_STOCK_STD
{ 
    public class TB_R_PART_HIKIATE_STOCK_STDInfo
	{
		#region "Public Members"
		public long ID { get; set; }
		public string PART_ID { get; set; }
		public int MIN_STOCK { get; set; }
		public int MAX_STOCK { get; set; }
		public DateTime? TC_FROM { get; set; }
		public string TC_FROM_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", TC_FROM);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public DateTime? TC_TO { get; set; }
		public string TC_TO_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", TC_TO);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public string IS_ACTIVE { get; set; }
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
		public TB_R_PART_HIKIATE_STOCK_STDInfo() 
		{
			ID = 0;
			PART_ID = "0";
			MIN_STOCK = 0;
			MAX_STOCK = 0;
			TC_FROM = null;
			TC_TO = null;
			IS_ACTIVE = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_R_PART_HIKIATE_STOCK_STDInfo(long id, string PART_ID, int MIN_STOCK, int MAX_STOCK, DateTime TC_FROM, DateTime TC_TO, string IS_ACTIVE, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
			this.PART_ID = PART_ID;
			this.MIN_STOCK = MIN_STOCK;
			this.MAX_STOCK = MAX_STOCK;
			this.TC_FROM = TC_FROM;
			this.TC_TO = TC_TO;
			this.IS_ACTIVE = IS_ACTIVE;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_ASSEMBLY_DATA
{ 
    public class TB_R_ASSEMBLY_DATAInfo
	{
		#region "Public Members"
		public long ID { get; set; }
		public string LINE { get; set; }
		public string PROCESS { get; set; }
		public string MODEL { get; set; }
		public string BODY_NO { get; set; }
		public string SEQ_NO { get; set; }
		public string GRADE { get; set; }
		public string LOT_NO { get; set; }
		public int NO_IN_LOT { get; set; }
		public string COLOR { get; set; }
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
		public int NO_IN_DATE { get; set; }
		public DateTime? A_IN_DATE_PLAN { get; set; }
		public string A_IN_DATE_PLAN_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", A_IN_DATE_PLAN);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public TimeSpan? A_IN_TIME_PLAN { get; set; }
		public DateTime? A_IN_DATE_ACTUAL { get; set; }
		public string A_IN_DATE_ACTUAL_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", A_IN_DATE_ACTUAL);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public TimeSpan? A_IN_TIME_ACTUAL { get; set; }
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
		public TB_R_ASSEMBLY_DATAInfo() 
		{
			ID = 0;
			LINE = string.Empty;
			PROCESS = string.Empty;
			MODEL = string.Empty;
			BODY_NO = string.Empty;
			SEQ_NO = string.Empty;
			GRADE = string.Empty;
			LOT_NO = string.Empty;
			NO_IN_LOT = 0;
			COLOR = string.Empty;
			WORKING_DATE = null;
			NO_IN_DATE = 0;
			A_IN_DATE_PLAN = null;
			A_IN_TIME_PLAN = null;
			A_IN_DATE_ACTUAL = null;
			A_IN_TIME_ACTUAL = null;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
		}
		
		public TB_R_ASSEMBLY_DATAInfo(long id, string LINE, string PROCESS, string MODEL, string BODY_NO, string SEQ_NO, string GRADE, string LOT_NO, int NO_IN_LOT, string COLOR, DateTime WORKING_DATE, int NO_IN_DATE, DateTime A_IN_DATE_PLAN, TimeSpan A_IN_TIME_PLAN, DateTime A_IN_DATE_ACTUAL, TimeSpan A_IN_TIME_ACTUAL, string CREATED_BY, DateTime CREATED_DATE, string UPDATED_BY, DateTime UPDATED_DATE)
		{
			this.ID = ID;
			this.LINE = LINE;
			this.PROCESS = PROCESS;
			this.MODEL = MODEL;
			this.BODY_NO = BODY_NO;
			this.SEQ_NO = SEQ_NO;
			this.GRADE = GRADE;
			this.LOT_NO = LOT_NO;
			this.NO_IN_LOT = NO_IN_LOT;
			this.COLOR = COLOR;
			this.WORKING_DATE = WORKING_DATE;
			this.NO_IN_DATE = NO_IN_DATE;
			this.A_IN_DATE_PLAN = A_IN_DATE_PLAN;
			this.A_IN_TIME_PLAN = A_IN_TIME_PLAN;
			this.A_IN_DATE_ACTUAL = A_IN_DATE_ACTUAL;
			this.A_IN_TIME_ACTUAL = A_IN_TIME_ACTUAL;
			this.CREATED_BY = CREATED_BY;
			this.CREATED_DATE = CREATED_DATE;
			this.UPDATED_BY = UPDATED_BY;
			this.UPDATED_DATE = UPDATED_DATE;
		}
		#endregion
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_USERS
{ 
    public class TB_M_USERSInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public string USER_ID { get; set; }
		public string USER_CC { get; set; }
		public String USER_NAME { get; set; }
		public Boolean ACTIVE { get; set; }
        public string TEAM_ID { get; set; }


		public DateTime? CREATE_DATE { get; set; }
		public string CREATE_DATE_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", CREATE_DATE);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		public DateTime? UPDATE_DATE { get; set; }
		public string UPDATE_DATE_Str_DDMMYYYY
		{
			get 
			{
				try
				{
					return string.Format("{0:dd/MM/yyyy}", UPDATE_DATE);
				}
				catch(Exception ex)
				{
					return "";
				}
			}
		}
		#endregion

 		public TB_M_USERSInfo() 
		{
			ID = 0;
			USER_CC = string.Empty;
			USER_NAME = string.Empty;
			ACTIVE = false;
			CREATE_DATE = null;
			UPDATE_DATE = null;
		}
     }
}



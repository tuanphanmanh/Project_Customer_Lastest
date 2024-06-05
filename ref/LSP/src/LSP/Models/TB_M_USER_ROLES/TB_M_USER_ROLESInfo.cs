using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_USER_ROLES
{ 
    public class TB_M_USER_ROLESInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public string USER_NAME { get; set; }
		public int TEAM_ID { get; set; }
		public string SHIFT { get; set; }
        
        public string TMV_CC { get; set; }
        public string TEAM { get; set; }
        public string CC_DESCRIPTION { get; set; }
        public string ROLE_SHIFT { get; set; }

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

		 
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_TMV_PIC
{ 
    public class TB_M_TMV_PICInfo
	{

		#region "Public Members"

		public long ID { get; set; }
        public long ROW_NO { get; set; }		   
        public string PIC_NAME { get; set; }
        public string PIC_TELEPHONE { get; set; }
        public string PIC_EMAIL { get; set; }
        public bool? PIC_EMAIL_BOL
        {
            get
            {
                return PIC_EMAIL == "Y" ? true : false;
            }
            set
            {
                if (value == true) PIC_EMAIL = "Y";
                else PIC_EMAIL = "N";
            }
        }
        public string IS_MAIN_PIC { get; set; }
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
        public string PIC_USER_ACCOUNT { get; set; }
        public string IS_SEND_EMAIL { get; set; }
        public bool? IS_SEND_EMAIL_BOL
        {
            get
            {
                return IS_SEND_EMAIL == "Y" ? true : false;
            }
            set
            {
                if (value == true) IS_SEND_EMAIL = "Y";
                else IS_SEND_EMAIL = "N";
            }
        }
        public string PIC_TELEPHONE_2 { get; set; }
        public string SUPPLIERS { get; set; }
       
		#endregion

		#region "Constructors"
        public TB_M_TMV_PICInfo() 
		{
			ID = 0;
            ROW_NO = 0;
            PIC_USER_ACCOUNT = string.Empty;
			PIC_NAME = string.Empty;
			PIC_TELEPHONE = string.Empty;
			PIC_EMAIL = string.Empty;
			IS_MAIN_PIC = null;
            IS_SEND_EMAIL = string.Empty;
            PIC_TELEPHONE_2 = string.Empty;
            IS_ACTIVE = string.Empty;       
            SUPPLIERS = string.Empty;
			CREATED_BY = string.Empty;
			CREATED_DATE = null;
			UPDATED_BY = string.Empty;
			UPDATED_DATE = null;
			    
           
		}

       
		#endregion
    }
}



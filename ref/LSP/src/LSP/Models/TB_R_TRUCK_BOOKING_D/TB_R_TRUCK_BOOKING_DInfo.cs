using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_TRUCK_BOOKING_D
{
    public class TB_R_TRUCK_BOOKING_DInfo
    {
        #region "Public Members"
		    public long ID { get; set; }
            public long ROW_NO { get; set; }

            public long BOOKING_H_ID { get; set; }            
            public long SUPPLIER_OR_TIME_ID { get; set; }

            public string SUPPLIER_CODE { get; set; }
            public int ORDER_SEQ { get; set; }
            public string ORDER_TYPE { get; set; }
            public TimeSpan? RECEIVE_TIME { get; set; }  

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
		    public TB_R_TRUCK_BOOKING_DInfo() 
		    {
			    ID = 0;
                ROW_NO = 0;

                BOOKING_H_ID = 0;                
                SUPPLIER_OR_TIME_ID = 0;

			    SUPPLIER_CODE = string.Empty;
                ORDER_SEQ = 0;
                ORDER_TYPE = string.Empty;
                RECEIVE_TIME = null;

                IS_ACTIVE   = string.Empty;
			    CREATED_BY  = string.Empty;
			    CREATED_DATE = null;
			    UPDATED_BY = string.Empty;
			    UPDATED_DATE = null;
		    }       
		#endregion
    }
}
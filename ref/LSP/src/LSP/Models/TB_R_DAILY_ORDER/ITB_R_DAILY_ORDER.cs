using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSP.Models.TB_M_LOOKUP;

namespace LSP.Models.TB_R_DAILY_ORDER
{
	public interface ITB_R_DAILY_ORDER
	{
		TB_R_DAILY_ORDERInfo TB_R_DAILY_ORDER_Get(string id);       
		
		IList<TB_R_DAILY_ORDERInfo> TB_R_DAILY_ORDER_Gets(string ID);    
				
		IList<TB_R_DAILY_ORDERInfo> TB_R_DAILY_ORDER_Search(TB_R_DAILY_ORDERInfo obj);

        IList<TB_R_DAILY_ORDERInfo> TB_R_DAILY_ORDER_Search_V2(TB_R_DAILY_ORDERInfo obj);
        
		int TB_R_DAILY_ORDER_Insert(TB_R_DAILY_ORDERInfo obj);
		
		int TB_R_DAILY_ORDER_Update(TB_R_DAILY_ORDERInfo obj);
		
		int TB_R_DAILY_ORDER_Delete(string id);

        IList<TB_R_DAILY_ORDER_PIVOTInfo> TB_R_DAILY_ORDER_GET_PIVOT_MONTH(TB_R_DAILY_ORDER_PIVOTInfo obj);
        IList<TB_R_DAILY_ORDER_PIVOTInfo> TB_R_DAILY_ORDER_GET_PIVOT_MONTH_V2(TB_R_DAILY_ORDER_PIVOTInfo obj);
        IList<TB_R_DAILY_ORDER_PIVOTInfo> TB_R_DAILY_ORDER_GET_PIVOT_MONTH_FC(TB_R_DAILY_ORDER_PIVOTInfo obj);

        int TB_R_DAILY_ORDER_GENERATE_MONTHLY(string SUPPLIER_NAME, string ORDER_FROM_DATE);
        int TB_R_DAILY_ORDER_GENERATE_MONTHLY_V2(string SUPPLIER_NAME, string ORDER_FROM_DATE, string IS_PP_OUT_CAL);
        int TB_R_DAILY_ORDER_GENERATE_KEIHEN_MONTHLY(string BASE_ORDER_ID);

        IList<TB_M_LOOKUPInfo> TB_R_DAILY_ORDER_CheckLockGenerate(string SUPPLIER_NAME, string ORDER_FROM_DATE);

        IList<TB_R_DAILY_ORDER_PIVOTInfo> TB_R_DAILY_ORDER_GET_GRN_MONTH(TB_R_DAILY_ORDER_PIVOTInfo obj);

        IList<TB_R_DAILY_ORDERInfo> TB_R_DAILY_ORDER_GET_ORDER_MULTI(string SUPPLIER_NAME, string ORDER_SEND_DATE, string USER_NAME);
    }
}


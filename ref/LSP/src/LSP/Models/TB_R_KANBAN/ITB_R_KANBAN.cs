using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_KANBAN
{
	public interface ITB_R_KANBAN
	{
		TB_R_KANBANInfo TB_R_KANBAN_Get(string id);       
		
		IList<TB_R_KANBANInfo> TB_R_KANBAN_Gets(string ID);
        IList<TB_R_KANBANInfo> TB_R_KANBAN_GetByOrderNo(string ORDER_NO);
        IList<TB_R_KANBANInfo> TB_R_KANBAN_GetByContentNo(string CONTENT_NO);
        IList<TB_R_KANBANInfo> TB_R_KANBAN_GetsByOrderID(string ORDER_ID);


		IList<TB_R_KANBANInfo> TB_R_KANBAN_Search(TB_R_KANBANInfo obj);
        IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByContentId(string CONTENT_LIST_ID);
        IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByContentIdDistinct(string CONTENT_LIST_ID);

        IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByOrderId(string ORDER_ID);
        IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByOrderIdDistinct(string ORDER_ID);
        IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByOrderIdMultiDistinct(string ORDER_ID);
        IList<TB_R_KANBANInfo> TB_R_KANBAN_SearchByOrderIdDistinctPKG(string ORDER_ID);
        
		int TB_R_KANBAN_Insert(TB_R_KANBANInfo obj);
        int TB_R_KANBAN_Import(TB_R_KANBANInfo obj);
		int TB_R_KANBAN_Update(TB_R_KANBANInfo obj);
		
		int TB_R_KANBAN_Delete(string id);
    }
}


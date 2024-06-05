using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_CALENDAR
{
    public interface ITB_M_CALENDAR
    {
        TB_M_CALENDARInfo TB_M_CALENDAR_Get(string id);

        IList<TB_M_CALENDARInfo> TB_M_CALENDAR_Gets(string ID);
        IList<TB_M_CALENDAR_PIVOTInfo> TB_M_CALENDAR_GET_PIVOT_MONTH(string WORKING_MONTH);
        IList<TB_M_CALENDAR_PIVOTInfo> TB_M_CALENDAR_GET_PIVOT_ORDER_RECIVE_DAY(string WORKING_MONTH);
        
        IList<TB_M_CALENDARInfo> TB_M_CALENDAR_Search(TB_M_CALENDARInfo obj);

        IList<TB_M_CALENDARInfo> TB_M_CALENDAR_SearchBySupplier(string SUPPLIER_CODE);

        int TB_M_CALENDAR_Insert(TB_M_CALENDARInfo obj);

        int TB_M_CALENDAR_Update(TB_M_CALENDARInfo obj);

        int TB_M_CALENDAR_Delete(string id);

        int TB_M_CALENDAR_Upload(DataTable _Calendar);

        int TB_M_CALENDAR_DeleteFuture(string SUPPLIER_CODE, string SYEAR);

        IEnumerable TB_M_CALENDAR_GetAppointments(TB_R_APPOINTMENTSInfo obj);
        IEnumerable TB_M_CALENDAR_GetResources(TB_R_RESOURCESInfo obj);
        SCHEDULER_DATAInfo TB_M_CALENDAR_GetSchedulerData(TB_R_APPOINTMENTSInfo obj1, TB_R_RESOURCESInfo obj2);
    }
}
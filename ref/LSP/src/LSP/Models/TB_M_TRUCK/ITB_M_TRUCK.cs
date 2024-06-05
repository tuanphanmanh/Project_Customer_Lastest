using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_TRUCK
{
    public interface ITB_M_TRUCK
    {
        TB_M_TRUCKInfo TB_M_TRUCK_Get(string id);

        IList<TB_M_TRUCKInfo> TB_M_TRUCK_GetsByTRUCKTYPE(TB_M_TRUCKInfo obj);

        IList<TB_M_TRUCKInfo> TB_M_TRUCK_Gets(string ID);

        IList<TB_M_TRUCKInfo> TB_M_TRUCK_Search(TB_M_TRUCKInfo obj);

        int TB_M_TRUCK_Insert(TB_M_TRUCKInfo obj);

        int TB_M_TRUCK_Update(TB_M_TRUCKInfo obj);

        int TB_M_TRUCK_Delete(string id);

        DataTable getTRUCKTYPE_ID();
    }
}
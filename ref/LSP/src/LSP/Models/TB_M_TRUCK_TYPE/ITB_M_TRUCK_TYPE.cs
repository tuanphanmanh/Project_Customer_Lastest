using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_TRUCK_TYPE
{
    public interface ITB_M_TRUCK_TYPE
    {
        TB_M_TRUCK_TYPEInfo TB_M_TRUCK_TYPE_Get(string id);

        IList<TB_M_TRUCK_TYPEInfo> TB_M_TRUCK_TYPE_Gets(string ID);

        IList<TB_M_TRUCK_TYPEInfo> TB_M_TRUCK_TYPE_Search(TB_M_TRUCK_TYPEInfo obj);

        int TB_M_TRUCK_TYPE_Insert(TB_M_TRUCK_TYPEInfo obj);

        int TB_M_TRUCK_TYPE_Update(TB_M_TRUCK_TYPEInfo obj);

        int TB_M_TRUCK_TYPE_Delete(string id);
    }
}
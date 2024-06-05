using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_VEHICLE_MASTER
{
    public interface ITB_M_VEHICLE_MASTER
    {
        TB_M_VEHICLE_MASTERInfo TB_M_VEHICLE_MASTER_Get(string id);

        IList<TB_M_VEHICLE_MASTERInfo> TB_M_VEHICLE_MASTER_GetsByModelID(TB_M_VEHICLE_MASTERInfo obj);

        IList<TB_M_VEHICLE_MASTERInfo> TB_M_VEHICLE_MASTER_Gets(string ID);

        IList<TB_M_VEHICLE_MASTERInfo> TB_M_VEHICLE_MASTER_Search(TB_M_VEHICLE_MASTERInfo obj);

        int TB_M_VEHICLE_MASTER_Insert(TB_M_VEHICLE_MASTERInfo obj);

        int TB_M_VEHICLE_MASTER_Update(TB_M_VEHICLE_MASTERInfo obj);

        int TB_M_VEHICLE_MASTER_Delete(string id);

        DataTable getMODEL_ID();
    }
}
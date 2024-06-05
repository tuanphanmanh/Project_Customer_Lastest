using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_TRUCK_BOOKING_D
{
    public interface ITB_R_TRUCK_BOOKING_D
    {
        TB_R_TRUCK_BOOKING_DInfo TB_R_TRUCK_BOOKING_D_Get(string id);

        IList<TB_R_TRUCK_BOOKING_DInfo> TB_R_TRUCK_BOOKING_D_GetsByBOOKING_H_ID(TB_R_TRUCK_BOOKING_DInfo obj);

        IList<TB_R_TRUCK_BOOKING_DInfo> TB_R_TRUCK_BOOKING_D_Gets(string ID);

        IList<TB_R_TRUCK_BOOKING_DInfo> TB_R_TRUCK_BOOKING_D_Search(TB_R_TRUCK_BOOKING_DInfo obj);

        int TB_R_TRUCK_BOOKING_D_Insert(TB_R_TRUCK_BOOKING_DInfo obj);

        int TB_R_TRUCK_BOOKING_D_Update(TB_R_TRUCK_BOOKING_DInfo obj);

        int TB_R_TRUCK_BOOKING_D_Delete(string id);

        DataTable getBOOKING_H_ID();
    }
}
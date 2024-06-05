using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_TRUCK_BOOKING_H
{
    public interface ITB_R_TRUCK_BOOKING_H
    {
        TB_R_TRUCK_BOOKING_HInfo TB_R_TRUCK_BOOKING_H_Get(string id);

        IList<TB_R_TRUCK_BOOKING_HInfo> TB_R_TRUCK_BOOKING_H_Gets(string ID);

        IList<TB_R_TRUCK_BOOKING_HInfo> TB_R_TRUCK_BOOKING_H_Search(TB_R_TRUCK_BOOKING_HInfo obj);        

        int TB_R_TRUCK_BOOKING_H_Insert(TB_R_TRUCK_BOOKING_HInfo obj);

        int TB_R_TRUCK_BOOKING_H_Update(TB_R_TRUCK_BOOKING_HInfo obj);

        int TB_R_TRUCK_BOOKING_H_Delete(string id);

    }
}
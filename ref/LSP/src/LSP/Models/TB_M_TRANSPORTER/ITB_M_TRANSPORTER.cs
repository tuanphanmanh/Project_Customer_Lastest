using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_M_TRANSPORTER
{
    public interface ITB_M_TRANSPORTER
    {
        TB_M_TRANSPORTERInfo TB_M_TRANSPORTER_Get(string id);

        IList<TB_M_TRANSPORTERInfo> TB_M_TRANSPORTER_Gets(string ID);

        IList<TB_M_TRANSPORTERInfo> TB_M_TRANSPORTER_Search(TB_M_TRANSPORTERInfo obj);

        int TB_M_TRANSPORTER_Insert(TB_M_TRANSPORTERInfo obj);

        int TB_M_TRANSPORTER_Update(TB_M_TRANSPORTERInfo obj);

        int TB_M_TRANSPORTER_Delete(string id);

    }
}
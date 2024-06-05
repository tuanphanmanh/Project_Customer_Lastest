using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PART_IN_OUT
{
    public interface ITB_R_PART_IN_OUT
    {
        TB_R_PART_IN_OUTInfo TB_R_PART_IN_OUT_Get (string id);

        IList<TB_R_PART_IN_OUTInfo> TB_R_PART_IN_OUT_GetsByPartID(TB_R_PART_IN_OUTInfo obj);

        IList<TB_R_PART_IN_OUTInfo> TB_R_PART_IN_OUT_Gets(string ID);

        IList<TB_R_PART_IN_OUTInfo> TB_R_PART_IN_OUT_Search(TB_R_PART_IN_OUTInfo obj);

        int TB_R_PART_IN_OUT_Insert(TB_R_PART_IN_OUTInfo obj);

        int TB_R_PART_IN_OUT_Update(TB_R_PART_IN_OUTInfo obj);

        int TB_R_PART_IN_OUT_Delete(string id);

        DataTable getPART_ID();
    }
}
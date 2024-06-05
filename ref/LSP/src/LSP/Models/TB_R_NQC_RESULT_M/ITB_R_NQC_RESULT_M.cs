using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_NQC_RESULT_M
{
    public interface ITB_R_NQC_RESULT_M
    {
        TB_R_NQC_RESULT_MInfo TB_R_NQC_RESULT_M_Get(string id);

        IList<TB_R_NQC_RESULT_MInfo> TB_R_NQC_RESULT_M_Gets(string id);

        IList<TB_R_NQC_RESULT_MInfo> TB_R_NQC_RESULT_M_Search(TB_R_NQC_RESULT_MInfo obj);

        int TB_R_NQC_RESULT_M_Insert(TB_R_NQC_RESULT_MInfo obj);

        int TB_R_NQC_RESULT_M_Update(TB_R_NQC_RESULT_MInfo obj);

        int TB_R_NQC_RESULT_M_Delete(string id);

        int TB_R_NQC_RESULT_M_Upload(DataTable _NQCResult);
    }
}
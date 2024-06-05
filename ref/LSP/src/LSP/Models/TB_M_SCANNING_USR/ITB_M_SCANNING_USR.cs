using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_SCANNING_USR
{
    public interface ITB_M_SCANNING_USR
	{
        TB_M_SCANNING_USRInfo TB_M_SCANNING_USR_Get(string id);

        IList<TB_M_SCANNING_USRInfo> TB_M_SCANNING_USR_Gets(string ID);

        IList<TB_M_SCANNING_USRInfo> TB_M_SCANNING_USR_Search(TB_M_SCANNING_USRInfo obj);

        int TB_M_SCANNING_USR_Insert(TB_M_SCANNING_USRInfo obj);

        int TB_M_SCANNING_USR_Update(TB_M_SCANNING_USRInfo obj);

        int TB_M_SCANNING_USR_Delete(string id);
    }
}


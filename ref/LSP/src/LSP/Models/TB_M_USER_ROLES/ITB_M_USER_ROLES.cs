using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_USER_ROLES
{
	public interface ITB_M_USER_ROLES
	{
		TB_M_USER_ROLESInfo TB_M_USER_ROLES_Get(string id);       
		
		IList<TB_M_USER_ROLESInfo> TB_M_USER_ROLES_Gets(string ID);
        IList<TB_M_USER_ROLESInfo> TB_M_USER_ROLES_GetsByUSER_ID(string USER_NAME);

		IList<TB_M_USER_ROLESInfo> TB_M_USER_ROLES_Search(TB_M_USER_ROLESInfo obj);
        
		int TB_M_USER_ROLES_Insert(TB_M_USER_ROLESInfo obj);
		
		int TB_M_USER_ROLES_Update(TB_M_USER_ROLESInfo obj);
		
		int TB_M_USER_ROLES_Delete(string id);
    }
}


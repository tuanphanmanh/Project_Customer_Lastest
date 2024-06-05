using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_USERS
{
	public interface ITB_M_USERS
	{
		TB_M_USERSInfo TB_M_USERS_Get(string id);
        TB_M_USERSInfo TB_M_USERS_GetByUserName(string USER_NAME);
		
		IList<TB_M_USERSInfo> TB_M_USERS_Gets(string ID);    
				
		IList<TB_M_USERSInfo> TB_M_USERS_Search(TB_M_USERSInfo obj);
        
		int TB_M_USERS_Insert(TB_M_USERSInfo obj);
		
		int TB_M_USERS_Update(TB_M_USERSInfo obj);
		
		int TB_M_USERS_Delete(string id);

        int TB_M_USERS_ChangePw(string pUsername, string pNewpassword);

        IList<ButtonInfo> TB_M_USERS_getSecurityButton(string App, string Roles, string Function);

    }
}


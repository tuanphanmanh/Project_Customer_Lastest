using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace LSP.Models.TB_M_SCREEN_SETTING
{
    public interface ITB_M_SCREEN_SETTING
    {
        TB_M_SCREEN_SETTINGInfo TB_M_SCREEN_SETTING_Get(string id);
        TB_M_SCREEN_SETTINGInfo TB_M_SCREEN_SETTING_GetByName(string SCREEN_NAME);

        IList<TB_M_SCREEN_SETTINGInfo> TB_M_SCREEN_SETTING_Gets(string ID);
     
        IList<TB_M_SCREEN_SETTINGInfo> TB_M_SCREEN_SETTING_Search(TB_M_SCREEN_SETTINGInfo obj);

        int TB_M_SCREEN_SETTING_Insert(TB_M_SCREEN_SETTINGInfo obj);

        int TB_M_SCREEN_SETTING_Update(TB_M_SCREEN_SETTINGInfo obj);               
        
        int TB_M_SCREEN_SETTING_Delete(string id);
        
        int TB_M_SCREEN_SETTING_UpdateByName(TB_M_SCREEN_SETTINGInfo obj);                
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_SCREEN_SETTING
{
    public class TB_M_SCREEN_SETTINGReposity: ITB_M_SCREEN_SETTING
    {
        public TB_M_SCREEN_SETTINGInfo TB_M_SCREEN_SETTING_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SCREEN_SETTINGInfo> list = db.Fetch<TB_M_SCREEN_SETTINGInfo>("TB_M_SCREEN_SETTING/TB_M_SCREEN_SETTING_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public TB_M_SCREEN_SETTINGInfo TB_M_SCREEN_SETTING_GetByName(string SCREEN_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SCREEN_SETTINGInfo> list = db.Fetch<TB_M_SCREEN_SETTINGInfo>("TB_M_SCREEN_SETTING/TB_M_SCREEN_SETTING_GetByName", 
                new { SCREEN_NAME = SCREEN_NAME });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
        
        public IList<TB_M_SCREEN_SETTINGInfo> TB_M_SCREEN_SETTING_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SCREEN_SETTINGInfo> list = db.Fetch<TB_M_SCREEN_SETTINGInfo>("TB_M_SCREEN_SETTING/TB_M_SCREEN_SETTING_Gets", new { id = ID });
            db.Close();
            return list;
        }        

        public IList<TB_M_SCREEN_SETTINGInfo> TB_M_SCREEN_SETTING_Search(TB_M_SCREEN_SETTINGInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SCREEN_SETTINGInfo> list = db.Fetch<TB_M_SCREEN_SETTINGInfo>("TB_M_SCREEN_SETTING/TB_M_SCREEN_SETTING_Search", new
            {
                SCREEN_NAME = obj.SCREEN_NAME                              
            });
            db.Close();
            return list;
        }

        public int TB_M_SCREEN_SETTING_Insert(TB_M_SCREEN_SETTINGInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SCREEN_SETTING/TB_M_SCREEN_SETTING_Insert", new
            {
                SCREEN_NAME = obj.SCREEN_NAME,
                SCREEN_TYPE = obj.SCREEN_TYPE,
                SCREEN_VALUE = obj.SCREEN_VALUE,               
                DESCRIPTION = obj.DESCRIPTION,
                IS_ACTIVE   = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                BARCODE_ID = obj.BARCODE_ID    
            });
            db.Close();
            return numrow;
        }

        public int TB_M_SCREEN_SETTING_Update(TB_M_SCREEN_SETTINGInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SCREEN_SETTING/TB_M_SCREEN_SETTING_Update", new
            {
                id = obj.ID,
                SCREEN_NAME = obj.SCREEN_NAME,
                SCREEN_TYPE = obj.SCREEN_TYPE,
                SCREEN_VALUE = obj.SCREEN_VALUE,             
                DESCRIPTION = obj.DESCRIPTION,                
                IS_ACTIVE   = obj.IS_ACTIVE,             
                UPDATED_BY = obj.UPDATED_BY,
                BARCODE_ID = obj.BARCODE_ID 
            });
            db.Close();
            return numrow;
        }

        public int TB_M_SCREEN_SETTING_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SCREEN_SETTING/TB_M_SCREEN_SETTING_Delete", new { id = id });
            db.Close();
            return numrow;
        }       

        public int TB_M_SCREEN_SETTING_UpdateByName(TB_M_SCREEN_SETTINGInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SCREEN_SETTING/TB_M_SCREEN_SETTING_UpdateByName", new
            {                
                SCREEN_NAME = obj.SCREEN_NAME,
                SCREEN_TYPE = obj.SCREEN_TYPE,
                SCREEN_VALUE = obj.SCREEN_VALUE,                                      
                UPDATED_BY = obj.UPDATED_BY,
                BARCODE_ID = obj.BARCODE_ID 
            });
            db.Close();
            return numrow;
        }      
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_USERS
{
    public class TB_M_USERSReposity : ITB_M_USERS
    {
        public TB_M_USERSInfo TB_M_USERS_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_USERSInfo> list = db.Fetch<TB_M_USERSInfo>("TB_M_USERS/TB_M_USERS_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public TB_M_USERSInfo TB_M_USERS_GetByUserName(string USER_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_USERSInfo> list = db.Fetch<TB_M_USERSInfo>("TB_M_USERS/TB_M_USERS_GetByUserName", new { USER_NAME = USER_NAME });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_M_USERSInfo> TB_M_USERS_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_USERSInfo> list = db.Fetch<TB_M_USERSInfo>("TB_M_USERS/TB_M_USERS_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_M_USERSInfo> TB_M_USERS_Search(TB_M_USERSInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_USERSInfo> list = db.Fetch<TB_M_USERSInfo>("TB_M_USERS/TB_M_USERS_Search", new { });
            db.Close();
            return list;
        }

        public int TB_M_USERS_Insert(TB_M_USERSInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_USERS/TB_M_USERS_Insert", new
            {
                USER_CC = obj.USER_CC,
                USER_NAME = obj.USER_NAME,
                ACTIVE = obj.ACTIVE,
                CREATE_DATE = obj.CREATE_DATE,
                UPDATE_DATE = obj.UPDATE_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_USERS_Update(TB_M_USERSInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_USERS/TB_M_USERS_Update", new
            {
                id = obj.ID,
                USER_CC = obj.USER_CC,
                USER_NAME = obj.USER_NAME,
                ACTIVE = obj.ACTIVE,
                CREATE_DATE = obj.CREATE_DATE,
                UPDATE_DATE = obj.UPDATE_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_USERS_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_USERS/TB_M_USERS_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        /// <summary>
        /// Change pw in SC
        /// </summary>
        /// <param name="pUsername"></param>
        /// <param name="pNewpassword"></param>
        /// <returns></returns>
        public int TB_M_USERS_ChangePw(string pUsername, string pNewpassword)
        {
            IDBContext db = DatabaseManager.Instance.GetContext(getConnectionName());
            int numrow = db.Execute("TB_M_USERS/TB_M_USERS_ChangePw", new
            {
                userName = pUsername,
                password = pNewpassword
            });
            db.Close();
            return numrow;
        }

        public IList<ButtonInfo> TB_M_USERS_getSecurityButton(string App, string Roles, string Function)
        {
            IDBContext db = DatabaseManager.Instance.GetContext(getConnectionName());
            IList<ButtonInfo> list = db.Fetch<ButtonInfo>("TB_M_USERS/TB_M_USERS_getSecurityButton", new
            {
                App = App,
                Roles = Roles,
                Function = Function
            }); 
            db.Close();
            return list;
        }

        private string getConnectionName()
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings["dbssoname"];
            }
            catch (Exception e)
            {
                return "SecurityCenter";
            }
        }

    }
}


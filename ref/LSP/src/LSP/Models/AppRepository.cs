using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models
{
    public class AppRepository
    {
        #region singleton
        private AppRepository() { }
        private static AppRepository instance = null;
        public static AppRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppRepository();
                }
                return instance;
            }
        }
        #endregion singleton

        #region voidprivate
        string nval = "";
        public int countApps()
        {
            int count = 0;
            IDBContext db = DatabaseManager.Instance.GetContext();
            try
            {
                dynamic args = new
                {
                    AppName = nval,
                    AppDesc = nval,
                    AppType = nval
                };
                count= db.SingleOrDefault<int>("countApplications", args);  
                db.Close();
            }
            catch (Exception e)
            {
                db.Close();
            }
            return count;
        }

        public List<App> getApps(int toNumber)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IEnumerable<App> result;
            try
            {
                dynamic args = new
                {
                    AppName = nval,
                    AppDesc = nval,
                    AppType = nval,
                    FromNumber = nval,
                    ToNumber = toNumber
                };

                result = db.Query<App>("getApplications", args);
                db.Close();
                return result.ToList();
            }
            catch (Exception e)
            {
                db.Close();
                return null;
            }
        }

        #endregion
    }
}
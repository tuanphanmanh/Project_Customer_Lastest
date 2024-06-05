using System;

namespace TMV.Common
{
    public class Constants
    {
        #region "Constructor"

        private static Constants _instance;
        private static System.Object _syncLock = new System.Object();

        protected Constants()
        {
        }

        public static Constants Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new Constants();
                }
            }
            return _instance;
        }

        protected void Dispose()
        {
            _instance = null;
        }

        #endregion

        #region Constant tables & table fields

        public class PROCESS_NAME
        {
            public static readonly string EKB_PICKING = "LGA_PIK_EKB";
        }

        public class BarcodePage
        {
            public static readonly string EKB_PICKING = "~/picking_ekb.aspx";            
        }

        public class BarcodeSession
        {           
            public static readonly string ALLOW_UNDO = "ALLOW_UNDO";
        }
                     
        public static readonly string APP_USERS_TBL = "APP_USERS";
        public class APP_USERS
        {
            #region "Constants"
            
            public static readonly string ID_COL = "Id";
            public static readonly string USER_ID_COL = "UserId";
            public static readonly string USER_NAME_COL = "UserName";
            public static readonly string PROCESS_ID_COL = "ProcessId";
            public static readonly string PROCESS_CODE_COL = "ProcessCode";
            #endregion
        }
                                    
        #endregion
    }
}

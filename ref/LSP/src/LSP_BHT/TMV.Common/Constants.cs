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

        #region "Admin"

        public string APP_FORMS_PKG_APP_FORMS_GETALL = "MA_FORMS_GetAll";
        public string APP_FORMS_PKG_APP_FORMS_GETBYID = "MA_FORMS_GetByID";

        #endregion

        #region Constant tables & table fields

        public class PROCESS_NAME
        {            
            public static readonly string UNLOADING = "UNLOADING";
            public static readonly string RECEIVING = "RECEIVING";    
            public static readonly string UNPACKING = "UNPACKING";
            public static readonly string UNPACKING_W = "UNPACKING_W";
            
        }



        public class BarcodePage
        {
            public static readonly string UNLOADING = "~/lsp_unloading.aspx";
            public static readonly string RECEIVING = "~/lsp_receiving.aspx";
            public static readonly string UNPACKING = "~/lsp_unpacking.aspx";
            public static readonly string UNPACKING_W = "~/lsp_unpacking_w.aspx";
        }

        public class BarcodeSession
        {           
            public static readonly string ALLOW_UNDO = "ALLOW_UNDO";
        }
                     
        public static readonly string APP_USERS_TBL = "APP_USERS";
        public class APP_USERS
        {
            #region "Constants"
            
            public static readonly string USERID_COL = "USERID";
            public static readonly string USERNAME_COL = "USERNAME";
            public static readonly string PROCESSID_COL= "PROCESSID";
            
            #endregion
        }
                                                      
        #endregion
    }
}

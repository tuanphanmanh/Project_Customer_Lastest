using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TMV.Common;
using TMV.DataAccess;

namespace TMV.ANDON.PLC
{
    public class InteractiveDB
    {
        #region "Constructor"
        private static InteractiveDB _instance;
        private static System.Object _syncLock = new System.Object();
        protected InteractiveDB()
        {
        }
        public static InteractiveDB Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new InteractiveDB();
                }
            }
            return _instance;
        }
        protected void Dispose()
        {
            _instance = null;
        }
        #endregion

        #region "PLC Event - "        
        public void BI_PLC_Data_LGAInsert(string pPLCData, string pLine, string pProcess)
        {
            SqlDataAccess.ExecuteNonQuery(SqlConnect.ConnectionString_LGA,
                "LGA_PLC_SIGNAL_INSERT", 
                new object[] { 
                    Globals.DB_GetNull(pPLCData), 
                    Globals.DB_GetNull(pLine), 
                    Globals.DB_GetNull(pProcess)                  
            });
        }

        public void BI_PLC_Data_LGWInsert(string pPLCData, string pLine, string pProcess)
        {
            SqlDataAccess.ExecuteNonQuery(SqlConnect.ConnectionString_LGW,
                "LGW_PLC_SIGNAL_INSERT",
                new object[] {
                    Globals.DB_GetNull(pPLCData),
                    Globals.DB_GetNull(pLine),
                    Globals.DB_GetNull(pProcess)
            });
        }

        public void BI_PLC_Data_LWAInsert(string pPLCData, string pLine, string pProcess)
        {
            SqlDataAccess.ExecuteNonQuery(SqlConnect.ConnectionString_LWA,
                "LGW_PLC_SIGNAL_INSERT", // same Store proc as LW above
                new object[] {
                    Globals.DB_GetNull(pPLCData),
                    Globals.DB_GetNull(pLine),
                    Globals.DB_GetNull(pProcess)
            });
        }

        #endregion
    }
}
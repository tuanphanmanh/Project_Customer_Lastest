using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.DataAccess;
using TMV.Common;
using System.Data;


public class LgaBarScanInfoBO
{

    #region "Constructor"
    private static LgaBarScanInfoBO _instance;
    private static System.Object _syncLock = new System.Object();
    protected LgaBarScanInfoBO()
    {
    }
    public static LgaBarScanInfoBO Instance()
    {
        if (_instance == null)
        {
            lock (_syncLock)
            {
                if (_instance == null)
                    _instance = new LgaBarScanInfoBO();
            }
        }
        return _instance;
    }
    protected void Dispose()
    {
        _instance = null;
    }
    #endregion

    #region "BO Functions"
 
    //3.Process picking Biz
    public void LgaBarScanInfoInsert(LgaBarScanInfo objInfo)
    {
        LgaBarScanInfoDAO.Instance().LgaBarScanInfoInsert(objInfo);
    }

    //5.Process check part label for P/K
    public DataSet LgaBarScanInfoCheckScanInfo(string SCAN_VALUE, string USER_ID)
    {
        return LgaBarScanInfoDAO.Instance().LgaBarScanInfoCheckScanInfo(SCAN_VALUE, USER_ID);
    }

    #endregion    


}

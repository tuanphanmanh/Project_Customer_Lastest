using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.DataAccess;
using TMV.Common;
using System.Data;


public class SCANNING_USRBO
{

    #region "Constructor"
    private static SCANNING_USRBO _instance;
    private static System.Object _syncLock = new System.Object();
    protected SCANNING_USRBO()
    {
    }
    public static SCANNING_USRBO Instance()
    {
        if (_instance == null)
        {
            lock (_syncLock)
            {
                 if (_instance == null)
                     _instance = new SCANNING_USRBO();
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

    public SCANNING_USRInfo GetById(string ID)
    {
        return SCANNING_USRDAO.Instance().GetById(ID);
    }


    public DataTable GetData(string id)
    {
        return SCANNING_USRDAO.Instance().GetData(id);
    }
    
    #endregion

}

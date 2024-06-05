using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.DataAccess;
using TMV.Common;
using System.Data;


public class MstLgaBarUserBO
{

    #region "Constructor"
    private static MstLgaBarUserBO _instance;
    private static System.Object _syncLock = new System.Object();
    protected MstLgaBarUserBO()
    {
    }
    public static MstLgaBarUserBO Instance()
    {
        if (_instance == null)
        {
            lock (_syncLock)
            {
                 if (_instance == null)
                     _instance = new MstLgaBarUserBO();
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

    public MstLgaBarUser GetById(string ID)
    {
        return MstLgaBarUserDAO.Instance().GetById(ID);
    }


    public DataTable GetData(string id)
    {
        return MstLgaBarUserDAO.Instance().GetData(id);
    }
    
    #endregion

}

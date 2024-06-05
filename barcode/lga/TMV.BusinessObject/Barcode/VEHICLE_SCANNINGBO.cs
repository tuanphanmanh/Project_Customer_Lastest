using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.DataAccess;
using TMV.Common;
using System.Data;


public class PART_SCANNINGBO
{

    #region "Constructor"
    private static PART_SCANNINGBO _instance;
    private static System.Object _syncLock = new System.Object();
    protected PART_SCANNINGBO()
    {
    }
    public static PART_SCANNINGBO Instance()
    {
        if (_instance == null)
        {
            lock (_syncLock)
            {
                if (_instance == null)
                    _instance = new PART_SCANNINGBO();
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

    
    public DataSet PROCESS_SCANNING_PART(string p_value, string p_user_id, string p_process_id)
    {
        return PART_SCANNINGDAO.Instance().PROCESS_SCANNING_PART(p_value, p_user_id, p_process_id);
    }
     
    #endregion    
}

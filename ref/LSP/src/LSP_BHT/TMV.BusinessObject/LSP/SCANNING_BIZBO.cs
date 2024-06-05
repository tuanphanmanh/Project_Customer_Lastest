using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMV.ObjectInfo;
using TMV.DataAccess;
using TMV.Common;
using System.Data;


public class SCANNING_BIZBO
{

    #region "Constructor"
    private static SCANNING_BIZBO _instance;
    private static System.Object _syncLock = new System.Object();
    protected SCANNING_BIZBO()
    {
    }
    public static SCANNING_BIZBO Instance()
    {
        if (_instance == null)
        {
            lock (_syncLock)
            {
                if (_instance == null)
                    _instance = new SCANNING_BIZBO();
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

    //1. Loading Truck
    public DataSet PROCESS_SCANNING_UL_TRUCK(string p_truck, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().PROCESS_SCANNING_UL_TRUCK(p_truck, p_user_id, p_process_id);
    }

    //2. Receiving
    public DataSet PROCESS_SCANNING_RE_ORDER(string p_order, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().PROCESS_SCANNING_RE_ORDER(p_order, p_user_id, p_process_id);
    }

    public DataSet PROCESS_SCANNING_RE_CONTENT(string p_order, string p_content, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().PROCESS_SCANNING_RE_CONTENT(p_order, p_content, p_user_id, p_process_id);
    }

    public bool Ad_ADHOC_CONTENT(string p_order, string p_content, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().Ad_ADHOC_CONTENT(p_order, p_content, p_user_id, p_process_id) > 0 ? true : false;
    }
    
    //3. Upacking
    public DataSet PROCESS_SCANNING_UP_CONTENT(string p_content, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().PROCESS_SCANNING_UP_CONTENT(p_content, p_user_id, p_process_id);
    }

    public DataSet PROCESS_SCANNING_UP_CONTENT_W(string p_content, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().PROCESS_SCANNING_UP_CONTENT_W(p_content, p_user_id, p_process_id);
    }

    public DataSet PROCESS_SCANNING_UP_PART(string p_part, string p_content, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().PROCESS_SCANNING_UP_PART(p_part, p_content, p_user_id, p_process_id);
    }

    public DataSet PROCESS_SCANNING_UP_PART_W(string p_part, string p_content, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().PROCESS_SCANNING_UP_PART_W(p_part, p_content, p_user_id, p_process_id);
    }

    public DataSet PROCESS_SCANNING_UP_FINISH(string p_FN, string p_content, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().PROCESS_SCANNING_UP_FINISH(p_FN, p_content, p_user_id, p_process_id);
    }

    public bool Update_CONTENT_FINISH(string p_content, string p_user_id, string p_process_id)
    {        
        return SCANNING_BIZDAO.Instance().Update_CONTENT_FINISH(p_content, p_user_id, p_process_id) > 0 ? true : false;
    }

    public bool Ad_ADHOC_KANBAN(string p_content, string p_part, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().Ad_ADHOC_KANBAN(p_content, p_part, p_user_id, p_process_id) > 0 ? true : false;
    }
    public bool Ad_ADHOC_KANBAN_W(string p_content, string p_part, string p_user_id, string p_process_id)
    {
        return SCANNING_BIZDAO.Instance().Ad_ADHOC_KANBAN_W(p_content, p_part, p_user_id, p_process_id) > 0 ? true : false;
    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_VEHICLE_MASTER
{
    public class TB_M_VEHICLE_MASTERReposity :ITB_M_VEHICLE_MASTER
    {
        public DataTable getMODEL_ID()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<object> list = db.Fetch<object>("TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_GetMODEL_ID");
            db.Close();
            return Common.Conver_Obj_Datatable(list);
        }

        public TB_M_VEHICLE_MASTERInfo TB_M_VEHICLE_MASTER_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_VEHICLE_MASTERInfo> list = db.Fetch<TB_M_VEHICLE_MASTERInfo>("TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_M_VEHICLE_MASTERInfo> TB_M_VEHICLE_MASTER_GetsByModelID(TB_M_VEHICLE_MASTERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_VEHICLE_MASTERInfo> list = db.Fetch<TB_M_VEHICLE_MASTERInfo>("TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_GetsByModelID", 
                new { MODEL_ID = obj.MODEL_ID });
            db.Close();
            return list;
        }

        public IList<TB_M_VEHICLE_MASTERInfo> TB_M_VEHICLE_MASTER_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_VEHICLE_MASTERInfo> list = db.Fetch<TB_M_VEHICLE_MASTERInfo>("TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_M_VEHICLE_MASTERInfo> TB_M_VEHICLE_MASTER_Search(TB_M_VEHICLE_MASTERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_VEHICLE_MASTERInfo> list = db.Fetch<TB_M_VEHICLE_MASTERInfo>("TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_Search",
            new
            {
                //USER_ID = obj.USER_ID,
                //USER_NAME = obj.USER_NAME
            });
            db.Close();
            return list;
        }

        public int TB_M_VEHICLE_MASTER_Insert(TB_M_VEHICLE_MASTERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_Insert", new
            {
                MODEL_ID = obj.MODEL_ID,
                CFC = obj.CFC,
                PROJECT_CODE = obj.PROJECT_CODE,
                KATASHIKI = obj.KATASHIKI,
                PROD_SFX = obj.PROD_SFX,
                MKT_SFX = obj.MKT_SFX,
                GRADE_MARK = obj.GRADE_MARK,
                START_LOT = obj.START_LOT,
                START_PROD_DATE = obj.START_PROD_DATE,
                END_LOT = obj.END_LOT,
                END_PROD_DATE = obj.END_PROD_DATE,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_VEHICLE_MASTER_Update(TB_M_VEHICLE_MASTERInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_Update", new
            {
                id = obj.ID,
                MODEL_ID = obj.MODEL_ID,
                CFC = obj.CFC,
                PROJECT_CODE = obj.PROJECT_CODE,
                KATASHIKI = obj.KATASHIKI,
                PROD_SFX = obj.PROD_SFX,
                MKT_SFX = obj.MKT_SFX,
                GRADE_MARK = obj.GRADE_MARK,
                START_LOT = obj.START_LOT,
                START_PROD_DATE = obj.START_PROD_DATE,
                END_LOT = obj.END_LOT,
                END_PROD_DATE = obj.END_PROD_DATE,
                IS_ACTIVE = obj.IS_ACTIVE,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE
            });
            db.Close();
            return numrow;
        }

        public int TB_M_VEHICLE_MASTER_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_UNLOADING_PLAN
{
	public class TB_R_UNLOADING_PLANReposity : ITB_R_UNLOADING_PLAN
	{
		public TB_R_UNLOADING_PLANInfo TB_R_UNLOADING_PLAN_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLANInfo> list = db.Fetch<TB_R_UNLOADING_PLANInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_UNLOADING_PLANInfo> TB_R_UNLOADING_PLAN_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLANInfo> list = db.Fetch<TB_R_UNLOADING_PLANInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_R_UNLOADING_PLANInfo> TB_R_UNLOADING_PLAN_GetsLINE()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLANInfo> list = db.Fetch<TB_R_UNLOADING_PLANInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsLINE", new { });
            db.Close();
            return list;
        }
        public IList<TB_R_UNLOADING_PLANInfo> TB_R_UNLOADING_PLAN_GetsLINE2()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLANInfo> list = db.Fetch<TB_R_UNLOADING_PLANInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsLINE2", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_UNLOADING_PLANInfo> TB_R_UNLOADING_PLAN_GetsLINE3()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLANInfo> list = db.Fetch<TB_R_UNLOADING_PLANInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsLINE3", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_UNLOADING_PLANInfo> TB_R_UNLOADING_PLAN_GetsLINE4()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLANInfo> list = db.Fetch<TB_R_UNLOADING_PLANInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsLINE4", new { });
            db.Close();
            return list;
        }

        public IList<TB_R_UNLOADING_PLANInfo> TB_R_UNLOADING_PLAN_GetsLINE4_DOCK(string SCREEN_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLANInfo> list = db.Fetch<TB_R_UNLOADING_PLANInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsLINE4_DOCK", 
                new {
                    SCREEN_NAME = SCREEN_NAME
                });
            db.Close();
            return list;
        }

        public IList<TB_R_UNLOADING_PLANInfo> TB_R_UNLOADING_PLAN_GetsLINE_DOCK(string SCREEN_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLANInfo> list = db.Fetch<TB_R_UNLOADING_PLANInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsLINE_DOCK", 
                new {
                    SCREEN_NAME = SCREEN_NAME
                });
            db.Close();
            return list;
        }

        public IList<UNLOADING_MAINInfo> TB_R_UNLOADING_PLAN_GetsDataByMAIN()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<UNLOADING_MAINInfo> list = db.Fetch<UNLOADING_MAINInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsDataByMAIN", new {    });
            db.Close();
            return list;
        }

        public IList<UNLOADING_MAINInfo> TB_R_UNLOADING_PLAN_GetsDataByMAIN_DOCK(string SCREEN_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<UNLOADING_MAINInfo> list = db.Fetch<UNLOADING_MAINInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsDataByMAIN_DOCK", 
                new {
                    SCREEN_NAME = SCREEN_NAME
                });
            db.Close();
            return list;
        }

        public IList<UNLOADING_MAINInfo> TB_R_UNLOADING_PLAN_GetsDataByMAIN2()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<UNLOADING_MAINInfo> list = db.Fetch<UNLOADING_MAINInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsDataByMAIN2", new { });
            db.Close();
            return list;
        }

        public IList<UNLOADING_MAINInfo> TB_R_UNLOADING_PLAN_GetsDataByMAIN3()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<UNLOADING_MAINInfo> list = db.Fetch<UNLOADING_MAINInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsDataByMAIN3", new { });
            db.Close();
            return list;
        }

        public IList<UNLOADING_MAINInfo> TB_R_UNLOADING_PLAN_GetsDataByMAIN4()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<UNLOADING_MAINInfo> list = db.Fetch<UNLOADING_MAINInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsDataByMAIN4", new { });
            db.Close();
            return list;
        }

        public IList<UNLOADING_MAINInfo> TB_R_UNLOADING_PLAN_GetsDataByMAIN4_DOCK(string SCREEN_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<UNLOADING_MAINInfo> list = db.Fetch<UNLOADING_MAINInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsDataByMAIN4_DOCK", 
                new {
                    SCREEN_NAME = SCREEN_NAME
                });
            db.Close();
            return list;
        }

        public IList<UNLOADING_MAINInfo> TB_R_UNLOADING_PLAN_GetsDataByMAIN4_DOCK_V2(string SCREEN_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<UNLOADING_MAINInfo> list = db.Fetch<UNLOADING_MAINInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_GetsDataByMAIN4_DOCK_V2",
                new
                {
                    SCREEN_NAME = SCREEN_NAME
                });
            db.Close();
            return list;
        }

		public IList<TB_R_UNLOADING_PLANInfo> TB_R_UNLOADING_PLAN_Search(TB_R_UNLOADING_PLANInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_UNLOADING_PLANInfo> list = db.Fetch<TB_R_UNLOADING_PLANInfo>("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_Search", new {
                WORKING_DATE = obj.WORKING_DATE,
                WORKING_DATE_FROM = obj.WORKING_DATE_FROM
            });
            db.Close();
            return list;
        }
		
		public int TB_R_UNLOADING_PLAN_Insert(TB_R_UNLOADING_PLANInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_Insert", new
            {
				DOCK = obj.DOCK,
				TRUCK = obj.TRUCK,
				SUPPLIERS = obj.SUPPLIERS,
				WORKING_DATE = obj.WORKING_DATE,
				SHIFT = obj.SHIFT,
				SEQUENCE_NO = obj.SEQUENCE_NO,
				PLAN_START_UP_DATETIME = obj.PLAN_START_UP_DATETIME,
				PLAN_FINISH_UP_DATETIME = obj.PLAN_FINISH_UP_DATETIME,
				ACTUAL_START_UP_DATETIME = obj.ACTUAL_START_UP_DATETIME,
				ACTUAL_FINISH_UP_DATETIME = obj.ACTUAL_FINISH_UP_DATETIME,
				REVISED_PLAN_START_UP_DATETIME = obj.REVISED_PLAN_START_UP_DATETIME,
				REVISED_PLAN_FINISH_UP_DATETIME = obj.REVISED_PLAN_FINISH_UP_DATETIME,
				ACTUAL_START_UP_DELAY = obj.ACTUAL_START_UP_DELAY,
				ACTUAL_FINISH_UP_DELAY = obj.ACTUAL_FINISH_UP_DELAY,
				STATUS = obj.STATUS,
				ISSUES = obj.ISSUES,
				CAUSE = obj.CAUSE,
				COUTERMEASURE = obj.COUTERMEASURE,
				PIC_RECORDER = obj.PIC_RECORDER,
				PIC_ACTION = obj.PIC_ACTION,
				ACTION_DUEDATE = obj.ACTION_DUEDATE,
				RESULT = obj.RESULT,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_UNLOADING_PLAN_Update(TB_R_UNLOADING_PLANInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_Update", new
            {
				id = obj.ID,
                DOCK = obj.DOCK,
				TRUCK = obj.TRUCK,
				SUPPLIERS = obj.SUPPLIERS,
				WORKING_DATE = obj.WORKING_DATE,
				SHIFT = obj.SHIFT,
				SEQUENCE_NO = obj.SEQUENCE_NO,
				PLAN_START_UP_DATETIME = obj.PLAN_START_UP_DATETIME,
				PLAN_FINISH_UP_DATETIME = obj.PLAN_FINISH_UP_DATETIME,
				ACTUAL_START_UP_DATETIME = obj.ACTUAL_START_UP_DATETIME,
				ACTUAL_FINISH_UP_DATETIME = obj.ACTUAL_FINISH_UP_DATETIME,
				REVISED_PLAN_START_UP_DATETIME = obj.REVISED_PLAN_START_UP_DATETIME,
				REVISED_PLAN_FINISH_UP_DATETIME = obj.REVISED_PLAN_FINISH_UP_DATETIME,
				ACTUAL_START_UP_DELAY = obj.ACTUAL_START_UP_DELAY,
				ACTUAL_FINISH_UP_DELAY = obj.ACTUAL_FINISH_UP_DELAY,
				STATUS = obj.STATUS,
				ISSUES = obj.ISSUES,
				CAUSE = obj.CAUSE,
				COUTERMEASURE = obj.COUTERMEASURE,
				PIC_RECORDER = obj.PIC_RECORDER,
				PIC_ACTION = obj.PIC_ACTION,
				ACTION_DUEDATE = obj.ACTION_DUEDATE,
				RESULT = obj.RESULT,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }

        public int TB_R_UNLOADING_PLAN_Insert_V2(TB_R_UNLOADING_PLANInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_Insert_V2", new
            {
                DOCK = obj.DOCK,
                TRUCK = obj.TRUCK,
                SUPPLIERS = obj.SUPPLIERS,
                SUPPLIERS_RETURN = obj.SUPPLIERS_RETURN,
                WORKING_DATE = obj.WORKING_DATE,
                SHIFT = obj.SHIFT,
                SEQUENCE_NO = obj.SEQUENCE_NO,
                PLAN_START_UP_DATETIME = obj.PLAN_START_UP_DATETIME,
                PLAN_FINISH_UP_DATETIME = obj.PLAN_FINISH_UP_DATETIME,
                ACTUAL_START_UP_DATETIME = obj.ACTUAL_START_UP_DATETIME,
                ACTUAL_FINISH_UP_DATETIME = obj.ACTUAL_FINISH_UP_DATETIME,
                REVISED_PLAN_START_UP_DATETIME = obj.REVISED_PLAN_START_UP_DATETIME,
                REVISED_PLAN_FINISH_UP_DATETIME = obj.REVISED_PLAN_FINISH_UP_DATETIME,
                ACTUAL_START_UP_DELAY = obj.ACTUAL_START_UP_DELAY,
                ACTUAL_FINISH_UP_DELAY = obj.ACTUAL_FINISH_UP_DELAY,
                STATUS = obj.STATUS,
                ISSUES = obj.ISSUES,
                CAUSE = obj.CAUSE,
                COUTERMEASURE = obj.COUTERMEASURE,
                PIC_RECORDER = obj.PIC_RECORDER,
                PIC_ACTION = obj.PIC_ACTION,
                ACTION_DUEDATE = obj.ACTION_DUEDATE,
                RESULT = obj.RESULT,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE,
                IS_ACTIVE = obj.IS_ACTIVE
            });
            db.Close();
            return numrow;
        }

        public int TB_R_UNLOADING_PLAN_Update_V2(TB_R_UNLOADING_PLANInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_Update_V2", new
            {
                id = obj.ID,
                DOCK = obj.DOCK,
                TRUCK = obj.TRUCK,
                SUPPLIERS = obj.SUPPLIERS,
                SUPPLIERS_RETURN = obj.SUPPLIERS_RETURN,
                WORKING_DATE = obj.WORKING_DATE,
                SHIFT = obj.SHIFT,
                SEQUENCE_NO = obj.SEQUENCE_NO,
                PLAN_START_UP_DATETIME = obj.PLAN_START_UP_DATETIME,
                PLAN_FINISH_UP_DATETIME = obj.PLAN_FINISH_UP_DATETIME,
                ACTUAL_START_UP_DATETIME = obj.ACTUAL_START_UP_DATETIME,
                ACTUAL_FINISH_UP_DATETIME = obj.ACTUAL_FINISH_UP_DATETIME,
                REVISED_PLAN_START_UP_DATETIME = obj.REVISED_PLAN_START_UP_DATETIME,
                REVISED_PLAN_FINISH_UP_DATETIME = obj.REVISED_PLAN_FINISH_UP_DATETIME,
                ACTUAL_START_UP_DELAY = obj.ACTUAL_START_UP_DELAY,
                ACTUAL_FINISH_UP_DELAY = obj.ACTUAL_FINISH_UP_DELAY,
                STATUS = obj.STATUS,
                ISSUES = obj.ISSUES,
                CAUSE = obj.CAUSE,
                COUTERMEASURE = obj.COUTERMEASURE,
                PIC_RECORDER = obj.PIC_RECORDER,
                PIC_ACTION = obj.PIC_ACTION,
                ACTION_DUEDATE = obj.ACTION_DUEDATE,
                RESULT = obj.RESULT,
                CREATED_BY = obj.CREATED_BY,
                CREATED_DATE = obj.CREATED_DATE,
                UPDATED_BY = obj.UPDATED_BY,
                UPDATED_DATE = obj.UPDATED_DATE,
                IS_ACTIVE = obj.IS_ACTIVE
            });
            db.Close();
            return numrow;
        }

		public int TB_R_UNLOADING_PLAN_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public int TB_R_UNLOADING_PLAN_ResetActual(string id, string UPDATED_BY)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_ResetActual", new { id = id, UPDATED_BY = UPDATED_BY});
            db.Close();
            return numrow;
        }
    }
}


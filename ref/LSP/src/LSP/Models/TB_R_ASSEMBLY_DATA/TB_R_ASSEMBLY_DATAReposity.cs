using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_ASSEMBLY_DATA
{
	public class TB_R_ASSEMBLY_DATAReposity : ITB_R_ASSEMBLY_DATA
	{
		public TB_R_ASSEMBLY_DATAInfo TB_R_ASSEMBLY_DATA_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_ASSEMBLY_DATAInfo> list = db.Fetch<TB_R_ASSEMBLY_DATAInfo>("TB_R_ASSEMBLY_DATA/TB_R_ASSEMBLY_DATA_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_ASSEMBLY_DATAInfo> TB_R_ASSEMBLY_DATA_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_ASSEMBLY_DATAInfo> list = db.Fetch<TB_R_ASSEMBLY_DATAInfo>("TB_R_ASSEMBLY_DATA/TB_R_ASSEMBLY_DATA_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_R_ASSEMBLY_DATAInfo> TB_R_ASSEMBLY_DATA_Search(TB_R_ASSEMBLY_DATAInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_ASSEMBLY_DATAInfo> list = db.Fetch<TB_R_ASSEMBLY_DATAInfo>("TB_R_ASSEMBLY_DATA/TB_R_ASSEMBLY_DATA_Search", new {
                LINE = obj.LINE,
                WORKING_DATE = obj.WORKING_DATE
            });
            db.Close();
            return list;
        }
		
		public int TB_R_ASSEMBLY_DATA_Insert(TB_R_ASSEMBLY_DATAInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_ASSEMBLY_DATA/TB_R_ASSEMBLY_DATA_Insert", new
            {
				LINE = obj.LINE,
				PROCESS = obj.PROCESS,
				MODEL = obj.MODEL,
				BODY_NO = obj.BODY_NO,
				SEQ_NO = obj.SEQ_NO,
				GRADE = obj.GRADE,
				LOT_NO = obj.LOT_NO,
				NO_IN_LOT = obj.NO_IN_LOT,
				COLOR = obj.COLOR,
				WORKING_DATE = obj.WORKING_DATE,
				NO_IN_DATE = obj.NO_IN_DATE,
				A_IN_DATE_PLAN = obj.A_IN_DATE_PLAN,
				A_IN_TIME_PLAN = obj.A_IN_TIME_PLAN,
				A_IN_DATE_ACTUAL = obj.A_IN_DATE_ACTUAL,
				A_IN_TIME_ACTUAL = obj.A_IN_TIME_ACTUAL,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_ASSEMBLY_DATA_Update(TB_R_ASSEMBLY_DATAInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_ASSEMBLY_DATA/TB_R_ASSEMBLY_DATA_Update", new
            {
				id = obj.ID,
                LINE = obj.LINE,
				PROCESS = obj.PROCESS,
				MODEL = obj.MODEL,
				BODY_NO = obj.BODY_NO,
				SEQ_NO = obj.SEQ_NO,
				GRADE = obj.GRADE,
				LOT_NO = obj.LOT_NO,
				NO_IN_LOT = obj.NO_IN_LOT,
				COLOR = obj.COLOR,
				WORKING_DATE = obj.WORKING_DATE,
				NO_IN_DATE = obj.NO_IN_DATE,
				A_IN_DATE_PLAN = obj.A_IN_DATE_PLAN,
				A_IN_TIME_PLAN = obj.A_IN_TIME_PLAN,
				A_IN_DATE_ACTUAL = obj.A_IN_DATE_ACTUAL,
				A_IN_TIME_ACTUAL = obj.A_IN_TIME_ACTUAL,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_ASSEMBLY_DATA_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_ASSEMBLY_DATA/TB_R_ASSEMBLY_DATA_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}


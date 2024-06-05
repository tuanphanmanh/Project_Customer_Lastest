using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_R_PART_SMQD
{
	public class TB_R_PART_SMQDReposity : ITB_R_PART_SMQD
	{
		public TB_R_PART_SMQDInfo TB_R_PART_SMQD_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_SMQDInfo> list = db.Fetch<TB_R_PART_SMQDInfo>("TB_R_PART_SMQD/TB_R_PART_SMQD_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_PART_SMQDInfo> TB_R_PART_SMQD_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_SMQDInfo> list = db.Fetch<TB_R_PART_SMQDInfo>("TB_R_PART_SMQD/TB_R_PART_SMQD_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_R_PART_SMQDInfo> TB_R_PART_SMQD_Search(TB_R_PART_SMQDInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_SMQDInfo> list = db.Fetch<TB_R_PART_SMQDInfo>("TB_R_PART_SMQD/TB_R_PART_SMQD_Search", new {

                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                YEAR = obj.YEAR,
                MONTH = obj.MONTH
            
            });
            db.Close();
            return list;
        }
		
		public int TB_R_PART_SMQD_Insert(TB_R_PART_SMQDInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_SMQD/TB_R_PART_SMQD_Insert", new
            {
                //PART_NO = obj.PART_NO,
                //COLOR_SFX = obj.COLOR_SFX,
                //PART_NAME = obj.PART_NAME,
                //BACK_NO = obj.BACK_NO,
                //SUPPLIER_CODE = obj.SUPPLIER_CODE,
                PART_ID = obj.PART_ID,
				SMQD_DATETIME = obj.SMQD_DATETIME,
				SMQD_QTY = obj.SMQD_QTY,
				SMQD_TYPE = obj.SMQD_TYPE,
				PIC = obj.PIC,
				RUN_NO = obj.RUN_NO,
				REASON = obj.REASON,
				STATUS = obj.STATUS,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE
                //UPDATED_BY = obj.UPDATED_BY,
                //UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_PART_SMQD_Update(TB_R_PART_SMQDInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_SMQD/TB_R_PART_SMQD_Update", new
            {
				id = obj.ID,
                //PART_NO = obj.PART_NO,
                //COLOR_SFX = obj.COLOR_SFX,
                //PART_NAME = obj.PART_NAME,
                //BACK_NO = obj.BACK_NO,
                //SUPPLIER_CODE = obj.SUPPLIER_CODE,
                PART_ID = obj.PART_ID,
				SMQD_DATETIME = obj.SMQD_DATETIME,
				SMQD_QTY = obj.SMQD_QTY,
				SMQD_TYPE = obj.SMQD_TYPE,
				PIC = obj.PIC,
				RUN_NO = obj.RUN_NO,
				REASON = obj.REASON,
				STATUS = obj.STATUS,
				IS_ACTIVE = obj.IS_ACTIVE,
                //CREATED_BY = obj.CREATED_BY,
                //CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }

        public int TB_R_PART_SMQD_Upload(TB_R_PART_SMQDInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_SMQD/TB_R_PART_SMQD_Upload", new
            { 
                PART_NO = obj.PART_NO,
				COLOR_SFX = obj.COLOR_SFX,
				PART_NAME = obj.PART_NAME, 
                BACK_NO = obj.BACK_NO,
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
                REASON = obj.REASON, 
				SMQD_DATETIME = obj.SMQD_DATETIME, 
				SMQD_QTY = obj.SMQD_QTY, 
				PIC = obj.PIC,
				RUN_NO = obj.RUN_NO, 
				UPDATED_BY = obj.UPDATED_BY
            });
            db.Close();
            return numrow;
        }

        
		
		public int TB_R_PART_SMQD_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_SMQD/TB_R_PART_SMQD_Delete", new { id = id });
            db.Close();
            return numrow;
        }
    }
}


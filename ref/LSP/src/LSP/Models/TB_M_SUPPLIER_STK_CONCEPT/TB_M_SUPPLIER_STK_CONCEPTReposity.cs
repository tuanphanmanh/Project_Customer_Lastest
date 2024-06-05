using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_SUPPLIER_STK_CONCEPT
{
	public class TB_M_SUPPLIER_STK_CONCEPTReposity : ITB_M_SUPPLIER_STK_CONCEPT
	{
		public TB_M_SUPPLIER_STK_CONCEPTInfo TB_M_SUPPLIER_STK_CONCEPT_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_STK_CONCEPTInfo> list = db.Fetch<TB_M_SUPPLIER_STK_CONCEPTInfo>("TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_SUPPLIER_STK_CONCEPTInfo> TB_M_SUPPLIER_STK_CONCEPT_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_STK_CONCEPTInfo> list = db.Fetch<TB_M_SUPPLIER_STK_CONCEPTInfo>("TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_M_SUPPLIER_STK_CONCEPTInfo> TB_M_SUPPLIER_STK_CONCEPT_Search(TB_M_SUPPLIER_STK_CONCEPTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_STK_CONCEPTInfo> list = db.Fetch<TB_M_SUPPLIER_STK_CONCEPTInfo>("TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_Search",
                new
                {
                    SUPPLIER_CODE = obj.SUPPLIER_CODE,
                    MONTH_STK = obj.MONTH_STK
                });
            db.Close();
            return list;
        }
		
		public int TB_M_SUPPLIER_STK_CONCEPT_Insert(TB_M_SUPPLIER_STK_CONCEPTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_Insert", new
            {
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				MONTH_STK = obj.MONTH_STK,
                STK_CONCEPT = obj.STK_CONCEPT,
                STK_CONCEPT_FRQ = obj.STK_CONCEPT_FRQ,
				MIN_STK_1 = obj.MIN_STK_1,              
				MIN_STK_2 = obj.MIN_STK_2,
				MIN_STK_3 = obj.MIN_STK_3,
				MIN_STK_4 = obj.MIN_STK_4,
				MIN_STK_5 = obj.MIN_STK_5,
				MIN_STK_6 = obj.MIN_STK_6,
				MIN_STK_7 = obj.MIN_STK_7,
				MIN_STK_8 = obj.MIN_STK_8,
				MIN_STK_9 = obj.MIN_STK_9,
				MIN_STK_10 = obj.MIN_STK_10,
				MIN_STK_11 = obj.MIN_STK_11,
				MIN_STK_12 = obj.MIN_STK_12,
				MIN_STK_13 = obj.MIN_STK_13,
				MIN_STK_14 = obj.MIN_STK_14,
				MIN_STK_15 = obj.MIN_STK_15,
				MAX_STK_1 = obj.MAX_STK_1,
				MAX_STK_2 = obj.MAX_STK_2,
				MAX_STK_3 = obj.MAX_STK_3,
				MAX_STK_4 = obj.MAX_STK_4,
				MAX_STK_5 = obj.MAX_STK_5,
				//MIN_STK_CONCEPT = obj.MIN_STK_CONCEPT,
				//MAX_STK_CONCEPT = obj.MAX_STK_CONCEPT,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_SUPPLIER_STK_CONCEPT_Update(TB_M_SUPPLIER_STK_CONCEPTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_Update", new
            {
				id = obj.ID,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
				MONTH_STK = obj.MONTH_STK,
                STK_CONCEPT = obj.STK_CONCEPT,
                STK_CONCEPT_FRQ = obj.STK_CONCEPT_FRQ,
				MIN_STK_1 = obj.MIN_STK_1,
				MIN_STK_2 = obj.MIN_STK_2,
				MIN_STK_3 = obj.MIN_STK_3,
				MIN_STK_4 = obj.MIN_STK_4,
				MIN_STK_5 = obj.MIN_STK_5,
				MIN_STK_6 = obj.MIN_STK_6,
				MIN_STK_7 = obj.MIN_STK_7,
				MIN_STK_8 = obj.MIN_STK_8,
				MIN_STK_9 = obj.MIN_STK_9,
				MIN_STK_10 = obj.MIN_STK_10,
				MIN_STK_11 = obj.MIN_STK_11,
				MIN_STK_12 = obj.MIN_STK_12,
				MIN_STK_13 = obj.MIN_STK_13,
				MIN_STK_14 = obj.MIN_STK_14,
				MIN_STK_15 = obj.MIN_STK_15,
				MAX_STK_1 = obj.MAX_STK_1,
				MAX_STK_2 = obj.MAX_STK_2,
				MAX_STK_3 = obj.MAX_STK_3,
				MAX_STK_4 = obj.MAX_STK_4,
				MAX_STK_5 = obj.MAX_STK_5,
				//MIN_STK_CONCEPT = obj.MIN_STK_CONCEPT,
				//MAX_STK_CONCEPT = obj.MAX_STK_CONCEPT,
				IS_ACTIVE = obj.IS_ACTIVE,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE 
            });
            db.Close();
            return numrow;
        }

        public int TB_M_SUPPLIER_STK_CONCEPT_Upload(TB_M_SUPPLIER_STK_CONCEPTInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_Upload", new
            { 
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                MONTH_STK = obj.MONTH_STK,
                STK_CONCEPT = obj.STK_CONCEPT,
                STK_CONCEPT_FRQ = obj.STK_CONCEPT_FRQ,
                MIN_STK_1 = obj.MIN_STK_1,
                MIN_STK_2 = obj.MIN_STK_2,
                MIN_STK_3 = obj.MIN_STK_3,
                MIN_STK_4 = obj.MIN_STK_4,
                MIN_STK_5 = obj.MIN_STK_5,
                MIN_STK_6 = obj.MIN_STK_6,
                MIN_STK_7 = obj.MIN_STK_7,
                MIN_STK_8 = obj.MIN_STK_8,
                MIN_STK_9 = obj.MIN_STK_9,
                MIN_STK_10 = obj.MIN_STK_10,
                MIN_STK_11 = obj.MIN_STK_11,
                MIN_STK_12 = obj.MIN_STK_12,
                MIN_STK_13 = obj.MIN_STK_13,
                MIN_STK_14 = obj.MIN_STK_14,
                MIN_STK_15 = obj.MIN_STK_15,
                MAX_STK_1 = obj.MAX_STK_1,
                MAX_STK_2 = obj.MAX_STK_2,
                MAX_STK_3 = obj.MAX_STK_3,
                MAX_STK_4 = obj.MAX_STK_4,
                MAX_STK_5 = obj.MAX_STK_5,
                MIN_STK_CONCEPT = obj.MIN_STK_CONCEPT,
                MAX_STK_CONCEPT = obj.MAX_STK_CONCEPT,
                IS_ACTIVE = obj.IS_ACTIVE, 
                UPDATED_BY = obj.UPDATED_BY 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_SUPPLIER_STK_CONCEPT_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public int TB_M_SUPPLIER_STK_CONCEPT_GENERATE_PART_DETAILS()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_GENERATE_PART_DETAILS", new {});
            db.Close();
            return numrow;
        }

        public int TB_M_SUPPLIER_STK_CONCEPT_GENERATE_BYCOPY_MONTH(string Month_Type)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_GENERATE_BYCOPY_MONTH", new { MONTH_TYPE = Month_Type });
            db.Close();
            return numrow;
        }
    }
}


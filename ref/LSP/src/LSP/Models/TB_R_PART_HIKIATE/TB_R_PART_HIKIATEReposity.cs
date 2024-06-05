using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using System.Data;
using System.Data.SqlClient;

namespace LSP.Models.TB_R_PART_HIKIATE
{
	public class TB_R_PART_HIKIATEReposity : ITB_R_PART_HIKIATE
	{
		public TB_R_PART_HIKIATEInfo TB_R_PART_HIKIATE_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_HIKIATEInfo> list = db.Fetch<TB_R_PART_HIKIATEInfo>("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_R_PART_HIKIATEInfo> TB_R_PART_HIKIATE_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_HIKIATEInfo> list = db.Fetch<TB_R_PART_HIKIATEInfo>("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_R_PART_HIKIATEInfo> TB_R_PART_HIKIATE_Search(TB_R_PART_HIKIATEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_HIKIATEInfo> list = db.Fetch<TB_R_PART_HIKIATEInfo>("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_Search", 
            new {
                CFC = obj.CFC,
                PROD_SFX = obj.PROD_SFX,
                PART_NO = obj.PART_NO,
                BACK_NO = obj.BACK_NO,
                SUPPLIER_CODE = obj.SUPPLIER_CODE
            });
            db.Close();
            return list;
        }

        public IList<TB_R_PART_HIKIATEInfo> TB_R_PART_HIKIATE_GetbySupplier(string SUPPLIER_CODE)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_HIKIATEInfo> list = db.Fetch<TB_R_PART_HIKIATEInfo>("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_GetbySupplier",
            new
            {
                SUPPLIER_CODE = SUPPLIER_CODE
            });
            db.Close();
            return list;
        }

        public IList<TB_R_PART_HIKIATEInfo> TB_R_PART_HIKIATE_SearchDetails(TB_R_PART_HIKIATEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_R_PART_HIKIATEInfo> list = db.Fetch<TB_R_PART_HIKIATEInfo>("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_SearchDetails",
            new
            {
                CFC = obj.CFC,
                PROD_SFX = obj.PROD_SFX,
                PART_NO = obj.PART_NO,
                BACK_NO = obj.BACK_NO,
                SUPPLIER_CODE = obj.SUPPLIER_CODE
            });
            db.Close();
            return list;
        }
		
		public int TB_R_PART_HIKIATE_Insert(TB_R_PART_HIKIATEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_Insert", new
            {
				CFC = obj.CFC,
				PROD_SFX = obj.PROD_SFX,
				PART_NO = obj.PART_NO,
				COLOR_SFX = obj.COLOR_SFX,
				PART_NAME = obj.PART_NAME,
				QTY_PER_VEHICLE = obj.QTY_PER_VEHICLE,
				BACK_NO = obj.BACK_NO,
				PARTS_MACHING_KEY = obj.PARTS_MACHING_KEY,
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				SHOP = obj.SHOP,
				DOCK = obj.DOCK,
				ORGANISATION = obj.ORGANISATION,
				RECEIVING_TIME = obj.RECEIVING_TIME,
				PLANT_TC_FROM = obj.PLANT_TC_FROM,
				PLANT_TC_TO = obj.PLANT_TC_TO,
				START_LOT = obj.START_LOT,
				END_LOT = obj.END_LOT,
				BOX_SIZE = obj.BOX_SIZE,
				PACKING_MIX = obj.PACKING_MIX,
				BOX_WEIGHT = obj.BOX_WEIGHT,
				BOX_W = obj.BOX_W,
				BOX_H = obj.BOX_H,
				BOX_L = obj.BOX_L,
				PALLET_WEIGHT = obj.PALLET_WEIGHT,
				QTY_BOX_PER_PALLET = obj.QTY_BOX_PER_PALLET,
				PALLET_W = obj.PALLET_W,
				PALLET_H = obj.PALLET_H,
				PALLET_L = obj.PALLET_L,
				UNIT = obj.UNIT,
				COST = obj.COST,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE ,
                DELIVERY_PROCESS = obj.DELIVERY_PROCESS,
                PACKAGING_TYPE = obj.PACKAGING_TYPE,
                COLOR = obj.COLOR
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_PART_HIKIATE_Update(TB_R_PART_HIKIATEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_Update", new
            {
				id = obj.ID,
                CFC = obj.CFC,
				PROD_SFX = obj.PROD_SFX,
				PART_NO = obj.PART_NO,
				COLOR_SFX = obj.COLOR_SFX,
				PART_NAME = obj.PART_NAME,
				QTY_PER_VEHICLE = obj.QTY_PER_VEHICLE,
				BACK_NO = obj.BACK_NO,
				PARTS_MACHING_KEY = obj.PARTS_MACHING_KEY,
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				SHOP = obj.SHOP,
				DOCK = obj.DOCK,
				ORGANISATION = obj.ORGANISATION,
				RECEIVING_TIME = obj.RECEIVING_TIME,
				PLANT_TC_FROM = obj.PLANT_TC_FROM,
				PLANT_TC_TO = obj.PLANT_TC_TO,
				START_LOT = obj.START_LOT,
				END_LOT = obj.END_LOT,
				BOX_SIZE = obj.BOX_SIZE,
				PACKING_MIX = obj.PACKING_MIX,
				BOX_WEIGHT = obj.BOX_WEIGHT,
				BOX_W = obj.BOX_W,
				BOX_H = obj.BOX_H,
				BOX_L = obj.BOX_L,
				PALLET_WEIGHT = obj.PALLET_WEIGHT,
				QTY_BOX_PER_PALLET = obj.QTY_BOX_PER_PALLET,
				PALLET_W = obj.PALLET_W,
				PALLET_H = obj.PALLET_H,
				PALLET_L = obj.PALLET_L,
				UNIT = obj.UNIT,
				COST = obj.COST,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE ,
                DELIVERY_PROCESS = obj.DELIVERY_PROCESS,
                PACKAGING_TYPE = obj.PACKAGING_TYPE,
                COLOR = obj.COLOR
            });
            db.Close();
            return numrow;
        }
		
		public int TB_R_PART_HIKIATE_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_Delete", new { id = id });
            db.Close();
            return numrow;
        }


        public int TB_R_PART_HIKIATE_UPLOAD(DataTable _TB_T_PART_HIKIATE)
        {
            int intReturn = 0;

            IList<ConnectionDescriptor> lists = DatabaseManager.Instance.GetConnectionDescriptors();
            string connectionString = lists[0].ConnectionString;

            using (SqlConnection cn = new SqlConnection { ConnectionString = connectionString })
            {
                cn.Open();
                using (SqlTransaction sqlTransaction = cn.BeginTransaction())
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, sqlTransaction))
                    {
                        //Set the database table name
                        sqlBulkCopy.DestinationTableName = "dbo.TB_T_PART_HIKIATE"; //insert into Temp table
                        try
                        {
                            sqlBulkCopy.BatchSize = 500; // The 500 value for SqlBulkCopy.BatchSize is also recommended
                            sqlBulkCopy.WriteToServer(_TB_T_PART_HIKIATE);
                            sqlTransaction.Commit();
                            intReturn = 1;
                        }
                        catch (Exception e)
                        {
                            sqlTransaction.Rollback();
                        }
                    }
                }
                cn.Close();
            }
            return intReturn;
        }

        public int TB_R_PART_HIKIATE_MERGE(string GUID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_MERGE", new { GUID = GUID });
            db.Close();
            return numrow;
        }


        public int TB_R_PART_HIKIATE_Update_ModuleCD(TB_R_PART_HIKIATEInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_Update_ModuleCD_V2", new
            {
                id = obj.ID,                
                MODULE_CD = obj.MODULE_CD,
                COLOR = obj.COLOR,
                REMARKS = obj.REMARKS,
                UPDATED_BY = obj.UPDATED_BY                                
            });
            db.Close();
            return numrow;
        }
    }
}


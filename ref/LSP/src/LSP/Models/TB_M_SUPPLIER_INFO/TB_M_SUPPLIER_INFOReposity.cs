using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_SUPPLIER_INFO
{
	public class TB_M_SUPPLIER_INFOReposity : ITB_M_SUPPLIER_INFO
	{
		public TB_M_SUPPLIER_INFOInfo TB_M_SUPPLIER_INFO_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_INFOInfo> list = db.Fetch<TB_M_SUPPLIER_INFOInfo>("TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }
		
		public IList<TB_M_SUPPLIER_INFOInfo> TB_M_SUPPLIER_INFO_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_INFOInfo> list = db.Fetch<TB_M_SUPPLIER_INFOInfo>("TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_Gets", new { id = ID });
            db.Close();
            return list;
        }
		
		public IList<TB_M_SUPPLIER_INFOInfo> TB_M_SUPPLIER_INFO_Search(TB_M_SUPPLIER_INFOInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_INFOInfo> list = db.Fetch<TB_M_SUPPLIER_INFOInfo>("TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_Search", 
            new { 
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
                SUPPLIER_NAME = obj.SUPPLIER_NAME
            });
            db.Close();
            return list;
        }
		
		public int TB_M_SUPPLIER_INFO_Insert(TB_M_SUPPLIER_INFOInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_Insert", new
            {
				SUPPLIER_CODE = obj.SUPPLIER_CODE,
				SUPPLIER_PLANT_CODE = obj.SUPPLIER_PLANT_CODE,
				SUPPLIER_NAME = obj.SUPPLIER_NAME,
                SUPPLIER_NAME_EN = obj.SUPPLIER_NAME_EN,
				ADDRESS = obj.ADDRESS,
				DOCK_X = obj.DOCK_X,
				DOCK_X_ADDRESS = obj.DOCK_X_ADDRESS,
				DELIVERY_METHOD = obj.DELIVERY_METHOD,
				DELIVERY_FREQUENCY = obj.DELIVERY_FREQUENCY,
				CD = obj.CD,
				ORDER_DATE_TYPE = obj.ORDER_DATE_TYPE,
				KEIHEN_TYPE = obj.KEIHEN_TYPE,
				STK_CONCEPT_TMV_MIN = obj.STK_CONCEPT_TMV_MIN,
				STK_CONCEPT_TMV_MAX = obj.STK_CONCEPT_TMV_MAX,
				STK_CONCEPT_SUP_M_MIN = obj.STK_CONCEPT_SUP_M_MIN,
				STK_CONCEPT_SUP_M_MAX = obj.STK_CONCEPT_SUP_M_MAX,
				STK_CONCEPT_SUP_P_MIN = obj.STK_CONCEPT_SUP_P_MIN,
				STK_CONCEPT_SUP_P_MAX = obj.STK_CONCEPT_SUP_P_MAX,
				TMV_PRODUCT_PERCENTAGE = obj.TMV_PRODUCT_PERCENTAGE,
				PIC_MAIN_ID = obj.PIC_MAIN_ID,
				DELIVERY_LT = obj.DELIVERY_LT,
				PRODUCTION_SHIFT = obj.PRODUCTION_SHIFT,
				TC_FROM = obj.TC_FROM,
				TC_TO = obj.TC_TO,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_SUPPLIER_INFO_Update(TB_M_SUPPLIER_INFOInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_Update", new
            {
				id = obj.ID,
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
				SUPPLIER_PLANT_CODE = obj.SUPPLIER_PLANT_CODE,
				SUPPLIER_NAME = obj.SUPPLIER_NAME,
                SUPPLIER_NAME_EN = obj.SUPPLIER_NAME_EN,
				ADDRESS = obj.ADDRESS,
				DOCK_X = obj.DOCK_X,
				DOCK_X_ADDRESS = obj.DOCK_X_ADDRESS,
				DELIVERY_METHOD = obj.DELIVERY_METHOD,
				DELIVERY_FREQUENCY = obj.DELIVERY_FREQUENCY,
				CD = obj.CD,
				ORDER_DATE_TYPE = obj.ORDER_DATE_TYPE,
				KEIHEN_TYPE = obj.KEIHEN_TYPE,
				STK_CONCEPT_TMV_MIN = obj.STK_CONCEPT_TMV_MIN,
				STK_CONCEPT_TMV_MAX = obj.STK_CONCEPT_TMV_MAX,
				STK_CONCEPT_SUP_M_MIN = obj.STK_CONCEPT_SUP_M_MIN,
				STK_CONCEPT_SUP_M_MAX = obj.STK_CONCEPT_SUP_M_MAX,
				STK_CONCEPT_SUP_P_MIN = obj.STK_CONCEPT_SUP_P_MIN,
				STK_CONCEPT_SUP_P_MAX = obj.STK_CONCEPT_SUP_P_MAX,
				TMV_PRODUCT_PERCENTAGE = obj.TMV_PRODUCT_PERCENTAGE,
				PIC_MAIN_ID = obj.PIC_MAIN_ID,
				DELIVERY_LT = obj.DELIVERY_LT,
				PRODUCTION_SHIFT = obj.PRODUCTION_SHIFT,
				TC_FROM = obj.TC_FROM,
				TC_TO = obj.TC_TO,
				CREATED_BY = obj.CREATED_BY,
				CREATED_DATE = obj.CREATED_DATE,
				UPDATED_BY = obj.UPDATED_BY,
				UPDATED_DATE = obj.UPDATED_DATE,
				IS_ACTIVE = obj.IS_ACTIVE 
            });
            db.Close();
            return numrow;
        }

        public int TB_M_SUPPLIER_INFO_Upload(TB_M_SUPPLIER_INFOInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_Upload", new
            { 
                SUPPLIER_CODE = obj.SUPPLIER_CODE,
				SUPPLIER_PLANT_CODE = obj.SUPPLIER_PLANT_CODE,
				SUPPLIER_NAME = obj.SUPPLIER_NAME,
                SUPPLIER_NAME_EN = obj.SUPPLIER_NAME_EN,
				ADDRESS = obj.ADDRESS,
				DOCK_X = obj.DOCK_X,
				DOCK_X_ADDRESS = obj.DOCK_X_ADDRESS,
				DELIVERY_METHOD = obj.DELIVERY_METHOD,
				DELIVERY_FREQUENCY = obj.DELIVERY_FREQUENCY,
				CD = obj.CD,
				ORDER_DATE_TYPE = obj.ORDER_DATE_TYPE,
				KEIHEN_TYPE = obj.KEIHEN_TYPE,
				STK_CONCEPT_TMV_MIN = obj.STK_CONCEPT_TMV_MIN,
				STK_CONCEPT_TMV_MAX = obj.STK_CONCEPT_TMV_MAX,
				STK_CONCEPT_SUP_M_MIN = obj.STK_CONCEPT_SUP_M_MIN,
				STK_CONCEPT_SUP_M_MAX = obj.STK_CONCEPT_SUP_M_MAX,
				STK_CONCEPT_SUP_P_MIN = obj.STK_CONCEPT_SUP_P_MIN,
				STK_CONCEPT_SUP_P_MAX = obj.STK_CONCEPT_SUP_P_MAX,
				TMV_PRODUCT_PERCENTAGE = obj.TMV_PRODUCT_PERCENTAGE,
				PIC_MAIN_ID = obj.PIC_MAIN_ID,
				DELIVERY_LT = obj.DELIVERY_LT,
				PRODUCTION_SHIFT = obj.PRODUCTION_SHIFT,
				TC_FROM = obj.TC_FROM,
				TC_TO = obj.TC_TO, 
				UPDATED_BY = obj.UPDATED_BY, 
				IS_ACTIVE = obj.IS_ACTIVE
            });
            db.Close();
            return numrow;
        }
		
		public int TB_M_SUPPLIER_INFO_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_Delete", new { id = id });
            db.Close();
            return numrow;
        }

        public IList<TB_M_SUPPLIER_INFOInfo> TB_M_SUPPLIER_INFO_GetsAllName()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_INFOInfo> list = db.Fetch<TB_M_SUPPLIER_INFOInfo>("TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_GetsAllName", new { });
            db.Close();
            return list;
        }

        public IList<TB_M_SUPPLIER_INFOInfo> TB_M_SUPPLIER_INFO_GetsAllNameByUser(string USER_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_SUPPLIER_INFOInfo> list = db.Fetch<TB_M_SUPPLIER_INFOInfo>("TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_GetsAllNameByUser",
                new { USER_NAME = USER_NAME });
            db.Close();
            return list;
        }
    }
}


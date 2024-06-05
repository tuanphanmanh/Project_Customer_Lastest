using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;

namespace LSP.Models.TB_M_LOOKUP
{
    public class TB_M_LOOKUPReposity : ITB_M_LOOKUP
    {
        public TB_M_LOOKUPInfo TB_M_LOOKUP_Get(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_LOOKUPInfo> list = db.Fetch<TB_M_LOOKUPInfo>("TB_M_LOOKUP/TB_M_LOOKUP_Get", new { id = id });
            db.Close();
            return list.Count > 0 ? list.First() : null;
        }

        public IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_Gets(string ID)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_LOOKUPInfo> list = db.Fetch<TB_M_LOOKUPInfo>("TB_M_LOOKUP/TB_M_LOOKUP_Gets", new { id = ID });
            db.Close();
            return list;
        }

        public IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_GetsByDOMAIN_CODE(string DOMAIN_CODE)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_LOOKUPInfo> list = db.Fetch<TB_M_LOOKUPInfo>("TB_M_LOOKUP/TB_M_LOOKUP_GetsByDOMAIN_CODE", new { DOMAIN_CODE = DOMAIN_CODE });
            db.Close();
            return list;
        }

        public IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_GetByDOMAIN_ITEMCODE(string DOMAIN_CODE, string ITEM_CODE)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_LOOKUPInfo> list = db.Fetch<TB_M_LOOKUPInfo>("TB_M_LOOKUP/TB_M_LOOKUP_GetByDOMAIN_ITEMCODE", new { DOMAIN_CODE = DOMAIN_CODE, ITEM_CODE = ITEM_CODE });
            db.Close();
            return list;
        }

        public IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_Search(TB_M_LOOKUPInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_LOOKUPInfo> list = db.Fetch<TB_M_LOOKUPInfo>("TB_M_LOOKUP/TB_M_LOOKUP_Search", new { DOMAIN_CODE = obj.DOMAIN_CODE, ITEM_CODE = obj.ITEM_CODE });
            db.Close();
            return list;
        }

        public int TB_M_LOOKUP_Insert(TB_M_LOOKUPInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_LOOKUP/TB_M_LOOKUP_Insert", new
            {
                DOMAIN_CODE = obj.DOMAIN_CODE,
                ITEM_CODE = obj.ITEM_CODE,
                ITEM_VALUE = obj.ITEM_VALUE,
                DESCRIPTION = obj.DESCRIPTION,
                IS_USE = obj.IS_USE,
                IS_RESTRICT = obj.IS_RESTRICT
            });
            db.Close();
            return numrow;
        }

        public int TB_M_LOOKUP_Update(TB_M_LOOKUPInfo obj)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_LOOKUP/TB_M_LOOKUP_Update", new
            {
                id = obj.ID,
                DOMAIN_CODE = obj.DOMAIN_CODE,
                ITEM_CODE = obj.ITEM_CODE,
                ITEM_VALUE = obj.ITEM_VALUE,
                DESCRIPTION = obj.DESCRIPTION,
                IS_USE = obj.IS_USE,
                IS_RESTRICT = obj.IS_RESTRICT
            });
            db.Close();
            return numrow;
        }

        public int TB_M_LOOKUP_Delete(string id)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int numrow = db.Execute("TB_M_LOOKUP/TB_M_LOOKUP_Delete", new { id = id });
            db.Close();
            return numrow;
        }


        public IList<TB_M_LOOKUPInfo> TB_M_LOOKUP_GetShiftByUserName(string pDOMAIN_CODE, string pshiftName)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            IList<TB_M_LOOKUPInfo> list = db.Fetch<TB_M_LOOKUPInfo>("TB_M_LOOKUP/TB_M_LOOKUP_GetShiftByUserName", new
            {
                DOMAIN_CODE = pDOMAIN_CODE,
                SHIFT_NAME = pshiftName
            });
            db.Close();
            return list;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using Toyota.Common;

namespace LSP.Models
{
    public class BaseRepository : IDisposable
    {
        Result result = new Result();
        public string NotExistFunction = System.Configuration.ConfigurationManager.AppSettings["NotExistFunction"];
        public string msgSuccessInsert = System.Configuration.ConfigurationManager.AppSettings["msgSuccessInsert"];
        public string msgSuccesssDelete = System.Configuration.ConfigurationManager.AppSettings["msgSuccesssDelete"];
        public string msgSuccesssUpdate = System.Configuration.ConfigurationManager.AppSettings["msgSuccesssUpdate"];
        public string msgWarningInsert = System.Configuration.ConfigurationManager.AppSettings["msgWarningInsert"];
        public string msgWarningDelete = System.Configuration.ConfigurationManager.AppSettings["msgWarningDelete"];
        public string msgSuccessReset = System.Configuration.ConfigurationManager.AppSettings["msgSuccessReset"];
        public string msgFailedReset = System.Configuration.ConfigurationManager.AppSettings["msgFailedReset"];
        public string msgFailedLogin = System.Configuration.ConfigurationManager.AppSettings["msgFailedLogin"];
        public string userNotFound = System.Configuration.ConfigurationManager.AppSettings["userNotFound"];
        public string emailNotFound = System.Configuration.ConfigurationManager.AppSettings["emailNotFound"];
        public string accountValidityMinus = System.Configuration.ConfigurationManager.AppSettings["accountValidityMinus"];
        public string passwordExpiration = System.Configuration.ConfigurationManager.AppSettings["passwordExpiration"];
        public string employeeUnder17 = System.Configuration.ConfigurationManager.AppSettings["employeeUnder17"];
        public string emailNotValid = System.Configuration.ConfigurationManager.AppSettings["emailNotValid"];
        public string extNotValid = System.Configuration.ConfigurationManager.AppSettings["extNotValid"];
        public string emptyFile = System.Configuration.ConfigurationManager.AppSettings["emptyFile"];
        public string uploadSuccess = System.Configuration.ConfigurationManager.AppSettings["uploadSuccess"];
        public string notSelected = System.Configuration.ConfigurationManager.AppSettings["notSelected"];
        public string pageNotFound = System.Configuration.ConfigurationManager.AppSettings["pageNotFound"];
        public string selectJustOne = System.Configuration.ConfigurationManager.AppSettings["selectJustOne"];
        public string changeApp = System.Configuration.ConfigurationManager.AppSettings["changeApp"];
        public string notContainSpace = System.Configuration.ConfigurationManager.AppSettings["notContainSpace"];
        public string SheetNotFound = System.Configuration.ConfigurationManager.AppSettings["SheetNotFound"];
        public string ErrorReaderExcel = System.Configuration.ConfigurationManager.AppSettings["ErrorReaderExcel"];
        public string extImageNotValid = System.Configuration.ConfigurationManager.AppSettings["extImageNotValid"];
        public static readonly string propDBKey = "SINGLE.DBCONTEXT";

        public static IDictionary CuCo
        {
            get
            {
                if (HttpContext.Current != null)
                    return HttpContext.Current.Items;
                else
                    return new Dictionary<string, object>();
            }
        }
        public static IDBContext Db
        {
            get
            {
                return CuCo[propDBKey] as IDBContext;
            }
        }

        public IDBContext db
        {
            get
            {
                if (Db == null)
                    CuCo[propDBKey] = DatabaseManager.Instance.GetContext();

                return Db;
            }
        }

        public void Dispose()
        {
            if (Db != null)
                Db.Close();
        }

        public Result MsgResultNotExistFunction(string functionname)
        {
            result.ResultCode = false;
            result.ResultDesc = NotExistFunction + "(" + functionname + ")";
            return result;
        }

        public Result MsgResultSuccessInsert()
        {
            result.ResultCode = true;
            result.ResultDesc = msgSuccessInsert;
            return result;
        }

        public Result MsgResultSuccessDelete()
        {
            result.ResultCode = true;
            result.ResultDesc = msgSuccesssDelete;
            return result;
        }

        public Result MsgResultSuccessUpdate()
        {
            result.ResultCode = true;
            result.ResultDesc = msgSuccesssUpdate;
            return result;
        }

        public Result MsgResultError(string msg)
        {
            result.ResultCode = false;

            if (msg.Length > 35)
            {
                if (msg.Substring(0, 35) == "Violation of PRIMARY KEY constraint")
                    result.ResultDesc = msgWarningInsert;
                else if (msg.Substring(0, 31) == "The DELETE statement conflicted")
                    result.ResultDesc = msgWarningDelete;
                else
                    result.ResultDesc = msg;
            }
            else
                result.ResultDesc = msg;

            return result;
        }

        public List<string> ExistingData(string id, string field, string table, string id2 = "", string field2 = "", string id3 = "", string field3 = "")//only for checking existing data when the action is insert
        {
            dynamic args = new
            {
                Field = field,
                Table = table,
                Id = id,
                Id2 = id2,
                Field2 = field2,
                Id3 = id3,
                Field3 = field3
            };
            IEnumerable<string> result = db.Query<string>("checkExistingData", args);
            return result.ToList();
        }


        #region fieldAndTable
        public string FieldId = "Id";
        public string FieldApp = "Application";
        public string FieldUsername = "Username";
        public string TBApplication = "TB_M_Application";
        public string TBRole = "TB_M_Role";
        public string TBFunction = "TB_M_Function";
        public string TBFeature = "TB_M_Feature";
        public string TBCompany = "TB_M_Company";
        public string TBUser = "TB_M_User";
        #endregion
    }
}
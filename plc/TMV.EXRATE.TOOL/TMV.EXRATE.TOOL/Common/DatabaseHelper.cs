using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMV.DataAccess;

namespace TMV.EXRATE.TOOL
{
    public class DatabaseHelper
    {
        #region "Constructor"
        private static DatabaseHelper _instance;
        private static System.Object _syncLock = new System.Object();
        protected DatabaseHelper()
        {
        }
        public static DatabaseHelper Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                        _instance = new DatabaseHelper();
                }
            }
            return _instance;
        }
        protected void Dispose()
        {
            _instance = null;
        }
        #endregion

        #region "DB"
        public void insertRow(Dictionary<string, string> fields, string guidStr)
        {
            
            string command = "insert into MstCmmExchangeRate_T2" +
                "(ExchangeDate, MajorCurrency, MinorCurrency, CeilingRate, SvbRate, FloorRate, BuyingOd, BuyingTt, SellingTtOd, Guid, AgvRate, ToolName, IsEmailReceived, EmailReceiveDateTime, CreationTime, Version) " +
                "values " +
                $"('{fields["ExchangeDate"]}', " +
                $"'{fields["MajorCurrency"]}', '{fields["MinorCurrency"]}', " +
                $"{fields["CeilingRate"]}, {fields["SvbRate"]}, {fields["FloorRate"]}, " +
                $"{fields["BuyingOd"]}, {fields["BuyingTt"]}, {fields["SellingTtOd"]}, '{guidStr}', {fields["AgvRate"]}, '{fields["ToolName"]}', '{fields["IsEmailReceived"]}', '{fields["EmailReceiveDateTime"]}', '{fields["CreationTime"]}', '{fields["Version"]}')";


            SqlDataAccess.ExecuteNonQuery(
                ConfigurationManager.AppSettings["ConnectSQL"].ToString(),
                System.Data.CommandType.Text, command
            );
        }

        public void callStore(string guid)
        {
            SqlDataAccess.ExecuteNonQuery(
                ConfigurationManager.AppSettings["ConnectSQL"].ToString(),
                AppConst.XCHECK_STORE,
                new object[]
                {
                    guid
                }
            );
        }

        public bool isExrateExist(string exchangeDate, string version)
        {
            var record = SqlDataAccess.ExecuteReader(
                 ConfigurationManager.AppSettings["ConnectSQL"].ToString(),
                 AppConst.CHECK_EXIST_STORE,
                 new object[]
                 {
                     2, /// 1 - MstCmmExchangeRate_T1, 2 - MstCmmExchangeRate_T2
                     exchangeDate,
                     version
                 }
            );

            return record.HasRows;
        }

        #endregion
    }
}

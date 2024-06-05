using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMV.DataAccess;
using RPA.EXRATE.TOOL.Models;

namespace RPA.EXRATE.TOOL
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
        public void InsertXRateData(List<ExchangeRate> exrates, string guidStr)
        {
            
            foreach (var field in exrates)
            {
                string command = "insert into MstCmmExchangeRate_T1" +
                "(ExchangeDate, MajorCurrency, MinorCurrency, CeilingRate, SvbRate, FloorRate, BuyingOd, BuyingTt, SellingTtOd, Guid, AgvRate, CreationTime, Version) " +
                "values " +
                $"('{field.ExchangeDate}', " +
                $"'{field.MajorCurrency}', '{field.MinorCurrency}', " +
                $"{field.CeilingRate}, {field.SvbRate}, {field.FloorRate}, " +
                $"{field.BuyingOd}, {field.BuyingTt}, {field.SellingTtOd}, '{guidStr}', {field.AgvRate}, '{field.CreationTime}', '{field.Version}')";


                SqlDataAccess.ExecuteNonQuery(
                    ConfigurationManager.AppSettings["ConnectSQL"].ToString(),
                    System.Data.CommandType.Text, command
                );
            }
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
                     1, /// 1 - MstCmmExchangeRate_T1, 2 - MstCmmExchangeRate_T2
                     exchangeDate,
                     version
                 }
            );

            return record.HasRows;
        }

        #endregion
    }
}

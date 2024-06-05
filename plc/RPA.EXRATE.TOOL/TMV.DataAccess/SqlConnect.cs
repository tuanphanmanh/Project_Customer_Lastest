using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace TMV.DataAccess
{
    public class SqlConnect
    {
        // Fields
        private static string _ConnectionString_LGA;
        private static string _ConnectionString_LGW;
        private static string _ConnectionString_LWA;
        private static string _ErrDescription;
        private static Dictionary<int, SqlTransaction> m_lstTransaction = new Dictionary<int, SqlTransaction>();
        private static Random m_rndTransaction = new Random(1);

        // Methods
        public static int BeginTransaction(string sKey)
        {            
            SqlConnection connection = new SqlConnection(GetConnectionString(sKey));
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            int key = m_rndTransaction.Next();
            m_lstTransaction.Add(key, transaction);
            return key;
        }

        public static bool CommitTransaction(int _TransactionID)
        {
            bool flag;
            try
            {
                SqlTransaction transactionFromID = GetTransactionFromID(_TransactionID);
                transactionFromID.Commit();

                if (transactionFromID.Connection != null)
                    transactionFromID.Connection.Close();

                transactionFromID.Dispose();
                m_lstTransaction.Remove(_TransactionID);
                flag = true;
            }
            catch (Exception ex)
            {
                _ErrDescription = ex.Message;
                flag = false;
                return flag;
            }
            return flag;
        }

        public static object GetConnectInfo_LGA(int _TransactionID)
        {
            if (!m_lstTransaction.ContainsKey(_TransactionID))
                return ConnectionString_LGA;

            return m_lstTransaction[_TransactionID];
        }

        public static object GetConnectInfo_LGW(int _TransactionID)
        {
            if (!m_lstTransaction.ContainsKey(_TransactionID))
                return ConnectionString_LGW;

            return m_lstTransaction[_TransactionID];
        }

        public static string GetConnectionString(string sKey)
        {
            return ConfigurationManager.AppSettings[sKey];
        }

        private static SqlTransaction GetTransactionFromID(int transactionID)
        {
            if (!m_lstTransaction.ContainsKey(transactionID))
                throw new Exception("Invalid transaction ID");

            return m_lstTransaction[transactionID];
        }

        public static bool RollbackTransaction(int _TransactionID)
        {
            bool flag;
            try
            {
                SqlTransaction transactionFromID = GetTransactionFromID(_TransactionID);
                transactionFromID.Rollback();

                if (transactionFromID.Connection != null)
                    transactionFromID.Connection.Close();

                transactionFromID.Dispose();
                m_lstTransaction.Remove(_TransactionID);
                flag = true;
            }
            catch (Exception ex)
            {
                _ErrDescription = ex.Message;
                flag = false;
                return flag;
            }
            return flag;
        }

        // Properties
        public static string ConnectionString_LGA
        {
            get
            {
                if (!string.IsNullOrEmpty(_ConnectionString_LGA))
                    return _ConnectionString_LGA;

                return ConfigurationManager.AppSettings["ConnectSQL_LGA"];
            }
            set
            {
                _ConnectionString_LGA = value;
            }
        }

        public static string ConnectionString_LGW
        {
            get
            {
                if (!string.IsNullOrEmpty(_ConnectionString_LGW))
                    return _ConnectionString_LGW;

                return ConfigurationManager.AppSettings["ConnectSQL_LGW"];
            }
            set
            {
                _ConnectionString_LGW = value;
            }
        }

        public static string ConnectionString_LWA
        {
            get
            {
                if (!string.IsNullOrEmpty(_ConnectionString_LWA))
                    return _ConnectionString_LWA;

                return ConfigurationManager.AppSettings["ConnectSQL_LWA"];
            }
            set
            {
                _ConnectionString_LWA = value;
            }
        }

        public static string ErrDescription
        {
            get
            {
                return _ErrDescription;
            }
            set
            {
                _ErrDescription = value;
            }
        }
    }
}

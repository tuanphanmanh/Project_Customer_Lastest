using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace TMV.DataAccess
{
    public sealed class SqlDataAccessParameterCache
    {
        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new OracleDataAccessParameterCache()".
        private SqlDataAccessParameterCache() { }

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// resolve at run time the appropriate set of SqlParameters for a stored procedure
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="spName"></param>
        /// <param name="includeReturnValueParameter"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connectionString, string spName, bool includeReturnValueParameter, params object[] parameterValues)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlCommand command = new SqlCommand(spName, connectionString)
            {
                CommandType = CommandType.StoredProcedure
            };
            connectionString.Open();
            SqlCommandBuilder.DeriveParameters(command);
            connectionString.Close();
            if (!includeReturnValueParameter & (command.Parameters[0].Direction == ParameterDirection.ReturnValue))
                command.Parameters.RemoveAt(0);

            SqlParameter[] array = new SqlParameter[(command.Parameters.Count - 1) + 1];
            command.Parameters.CopyTo(array, 0);
            foreach (SqlParameter parameter in array)
            {
                parameter.Value = DBNull.Value;
            }
            return array;
        }

        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }
            return clonedParameters;
        }

        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((commandText == null) || (commandText.Length == 0))
                throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;
            paramCache[hashKey] = commandParameters;
        }

        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((commandText == null) || (commandText.Length == 0))
                throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];

            if (cachedParameters == null)
                return null;
            else
                return CloneParameters(cachedParameters);
        }

        public static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        public static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            using (SqlConnection connection2 = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(connection2, spName, includeReturnValueParameter);
            }
        }

        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
            SqlParameter[] originalParameters = (SqlParameter[])paramCache[hashKey];
            if (originalParameters == null)
            {
                SqlParameter[] parameterArray3 = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter, new object[0]);
                paramCache[hashKey] = parameterArray3;
                originalParameters = parameterArray3;
            }
            return CloneParameters(originalParameters);
        }

        internal static void RemoveParameterCache(string connectionString, string spName)
        {
            string key = connectionString + ":" + spName;

            if (paramCache.ContainsKey(key))
                paramCache.Remove(key);
        }
    }
}

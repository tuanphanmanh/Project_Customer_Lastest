using System;
using System.Collections;
using System.Data;
using System.Xml;
using System.Threading;
using System.Data.SqlClient;

namespace TMV.DataAccess
{
    public sealed class SqlDataAccess
    {
        #region private uLGWity methods & constructors

        private SqlDataAccess() { }

        // Nested Types
        private enum SqlConnectionOwnership
        {
            Internal,
            External
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters != null) && (dataRow != null))
            {
                foreach (SqlParameter parameter in commandParameters)
                {
                    int num = 0;
                    if ((parameter.ParameterName == null) || (parameter.ParameterName.Length <= 1))
                        throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: ' {1}' .", num, parameter.ParameterName));

                    if (dataRow.Table.Columns.IndexOf(parameter.ParameterName.Substring(1)) != -1)
                        parameter.Value = dataRow[parameter.ParameterName.Substring(1)];

                    num++;
                }
            }
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters != null) || (parameterValues != null))
            {
                if (commandParameters.Length != parameterValues.Length)
                    throw new ArgumentException("Parameter count does not match Parameter Value count.");

                for (int i = 0; i <= commandParameters.Length - 1; i++)
                {
                    if (parameterValues[i] is IDbDataParameter)
                    {
                        IDbDataParameter parameter = (IDbDataParameter)parameterValues[i];
                        if (parameter.Value == null)
                            commandParameters[i].Value = DBNull.Value;
                        else
                            commandParameters[i].Value = parameter.Value;
                    }
                    else if (parameterValues[i] == null)
                        commandParameters[i].Value = DBNull.Value;
                    else
                        commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (commandParameters != null)
            {
                foreach (SqlParameter parameter in commandParameters)
                {
                    if (parameter != null)
                    {
                        if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                            parameter.Value = DBNull.Value;

                        command.Parameters.Add(parameter);
                    }
                }
            }
        }

        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            SqlCommand command = new SqlCommand(spName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);

                for (int i = 0; i <= sourceColumns.Length - 1; i++)
                {
                    spParameterSet[i].SourceColumn = sourceColumns[i];
                }
                AttachParameters(command, spParameterSet);
            }
            return command;
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, ref bool mustCloseConnection)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if ((commandText == null) || (commandText.Length == 0))
                throw new ArgumentNullException("commandText");

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                mustCloseConnection = true;
            }
            else
                mustCloseConnection = false;

            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                if (transaction.Connection == null)
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
                AttachParameters(command, commandParameters);
        }

        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if (insertCommand == null)
                throw new ArgumentNullException("insertCommand");

            if (deleteCommand == null)
                throw new ArgumentNullException("deleteCommand");

            if (updateCommand == null)
                throw new ArgumentNullException("updateCommand");

            if (dataSet == null)
                throw new ArgumentNullException("dataSet");

            if ((tableName == null) || (tableName.Length == 0))
                throw new ArgumentNullException("tableName");

            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                adapter.UpdateCommand = updateCommand;
                adapter.InsertCommand = insertCommand;
                adapter.DeleteCommand = deleteCommand;
                adapter.Update(dataSet, tableName);
                dataSet.AcceptChanges();
            }
        }

        #endregion

        #region ExecuteNonQuery

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                try
                {
                    AssignParameterValues(spParameterSet, parameterValues);
                }
                catch (Exception ex)
                {
                    SqlDataAccessParameterCache.RemoveParameterCache(connection.ConnectionString, spName);
                    throw ex;
                }
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                try
                {
                    AssignParameterValues(spParameterSet, parameterValues);
                }
                catch (Exception ex)
                {
                    SqlDataAccessParameterCache.RemoveParameterCache(transaction.Connection.ConnectionString, spName);
                    throw ex;
                }
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                try
                {
                    AssignParameterValues(spParameterSet, parameterValues);
                }
                catch (Exception ex)
                {
                    SqlDataAccessParameterCache.RemoveParameterCache(connectionString, spName);
                    throw ex;
                }
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters, ref mustCloseConnection);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();

            return num;
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return num;
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        public static int ExecuteNonQueryTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQueryTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }

        #endregion

        #region ExecuteDataSet

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            SqlCommand command = new SqlCommand();
            DataSet dataSet = new DataSet();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters, ref mustCloseConnection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(dataSet);
                command.Parameters.Clear();
            }
            if (mustCloseConnection)
                connection.Close();

            return dataSet;
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand command = new SqlCommand();
            DataSet dataSet = new DataSet();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(dataSet);
                command.Parameters.Clear();
            }
            return dataSet;
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDatasetTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }

        #endregion

        #region ExecuteReader

        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                try
                {
                    AssignParameterValues(spParameterSet, parameterValues);
                }
                catch (Exception ex)
                {
                    SqlDataAccessParameterCache.RemoveParameterCache(connection.ConnectionString, spName);
                    throw ex;
                }
                return ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                try
                {
                    AssignParameterValues(spParameterSet, parameterValues);
                }
                catch (Exception ex)
                {
                    SqlDataAccessParameterCache.RemoveParameterCache(transaction.Connection.ConnectionString, spName);
                    throw ex;
                }
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, null);
        }

        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                try
                {
                    AssignParameterValues(spParameterSet, parameterValues);
                }
                catch (Exception ex)
                {
                    SqlDataAccessParameterCache.RemoveParameterCache(connectionString, spName);
                    throw ex;
                }
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection connection = null;
            SqlDataReader reader;
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                reader = ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch (Exception ex)
            {
                if (connection != null)
                    connection.Dispose();

                throw ex;
            }
            return reader;
        }

        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            SqlDataReader reader;
            if (connection == null)
                throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;
            SqlCommand command = new SqlCommand();
            try
            {
                SqlDataReader reader2 = null;
                PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
                bool flag3 = false;
                int num = 0;
                do
                {
                    try
                    {
                        if (connectionOwnership == SqlConnectionOwnership.External)
                            reader2 = command.ExecuteReader();
                        else
                            reader2 = command.ExecuteReader(CommandBehavior.CloseConnection);

                        flag3 = true;
                    }
                    catch (Exception ex)
                    {
                        if (!(((ex is InvalidOperationException) & (ex.Message == "There is already an open DataReader associated with this Connection which must be closed first.")) & (ex.Source == "System.Data")))
                            throw ex;

                        num++;
                        if (num > 10)
                            throw ex;

                        Thread.Sleep(500);
                    }
                }
                while (!flag3);

                bool flag2 = true;
                foreach (SqlParameter current in command.Parameters)
                {
                    if (current.Direction != ParameterDirection.Input)
                        flag2 = false;
                }

                if (flag2)
                    command.Parameters.Clear();

                reader = reader2;
            }
            catch (Exception ex)
            {
                if (mustCloseConnection)
                    connection.Close();

                throw ex;
            }
            return reader;
        }

        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }

        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }

        public static SqlDataReader ExecuteReaderTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }

        #endregion

        #region ExecuteScalar

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, null);
        }

        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                try
                {
                    AssignParameterValues(spParameterSet, parameterValues);
                }
                catch (Exception ex)
                {
                    SqlDataAccessParameterCache.RemoveParameterCache(connection.ConnectionString, spName);
                    throw ex;
                }
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, null);
        }

        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                try
                {
                    AssignParameterValues(spParameterSet, parameterValues);
                }
                catch (Exception ex)
                {
                    SqlDataAccessParameterCache.RemoveParameterCache(transaction.Connection.ConnectionString, spName);
                    throw ex;
                }
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connectionString, commandType, commandText, null);
        }

        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                try
                {
                    AssignParameterValues(spParameterSet, parameterValues);
                }
                catch (Exception ex)
                {
                    SqlDataAccessParameterCache.RemoveParameterCache(connectionString, spName);
                    throw ex;
                }
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters, ref mustCloseConnection);
            object objectValue = command.ExecuteScalar();
            command.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();

            return objectValue;
        }

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            object objectValue = command.ExecuteScalar();
            command.Parameters.Clear();
            return objectValue;
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }

        public static object ExecuteScalarTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalarTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalarTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }

        #endregion

        #region ExecuteXmlReader

        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(connection, commandType, commandText, null);
        }

        public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(transaction, commandType, commandText, null);
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }

        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            XmlReader reader;
            if (connection == null)
                throw new ArgumentNullException("connection");

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            try
            {
                PrepareCommand(command, connection, null, commandType, commandText, commandParameters, ref mustCloseConnection);
                XmlReader reader2 = command.ExecuteXmlReader();
                command.Parameters.Clear();
                reader = reader2;
            }
            catch (Exception ex)
            {
                if (mustCloseConnection)
                    connection.Close();

                throw ex;
            }
            return reader;
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            XmlReader xmlRead = command.ExecuteXmlReader();
            command.Parameters.Clear();
            return xmlRead;
        }

        public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }

        public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(spParameterSet, dataRow);
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }

        #endregion

        #region FillDataSet

        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        public static void FillDataset(SqlConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if (dataSet == null)
                throw new ArgumentNullException("dataSet");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
            }
            else
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
        }

        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        public static void FillDataset(SqlTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            if (dataSet == null)
                throw new ArgumentNullException("dataSet");

            if ((spName == null) || (spName.Length == 0))
                throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] spParameterSet = SqlDataAccessParameterCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(spParameterSet, parameterValues);
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
            }
            else
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
        }

        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            SqlConnection connection = null;
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if (dataSet == null)
                throw new ArgumentNullException("dataSet");

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }

        public static void FillDataset(string connectionString, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            SqlConnection connection = null;
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if (dataSet == null)
                throw new ArgumentNullException("dataSet");

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                FillDataset(connection, spName, dataSet, tableNames, parameterValues);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }

        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            SqlConnection connection = null;
            if ((connectionString == null) || (connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");

            if (dataSet == null)
                throw new ArgumentNullException("dataSet");

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        }

        private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if (dataSet == null)
                throw new ArgumentNullException("dataSet");

            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if ((tableNames != null) && (tableNames.Length > 0))
                {
                    string sourceTable = "Table";
                    for (int i = 0; i <= tableNames.Length - 1; i++)
                    {
                        if ((tableNames[i] == null) || (tableNames[i].Length == 0))
                        {
                            throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        }
                        adapter.TableMappings.Add(sourceTable, tableNames[i]);
                        sourceTable = sourceTable + ((i + 1)).ToString();
                    }
                }
                adapter.Fill(dataSet);
                command.Parameters.Clear();
            }
            if (mustCloseConnection)
                connection.Close();
        }

        #endregion
    }
}

using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DAL
{
    internal abstract class BaseDao
    {
        const string ConnectionStringName = "ChapeauDatabase";
        const string DatabaseErrorMessage = "Database operation failed.";
        protected const int Zero = 0;

        private SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            connection.Open();
            return connection;
        }

        protected List<T> ReadTable<T>(DataTable dataTable, Func<DataRow, T> readRow)
        {
            List<T> list = new List<T>();

            foreach (DataRow dr in dataTable.Rows)
            {
                T item = readRow(dr);
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// For Create/Update/Delete Queries.
        /// </summary>
        protected void ExecuteEditQuery(string query, SqlParameter[] sqlParameters)
        {
            try
            {
                using (SqlConnection connection = OpenConnection())
                {
                    using (SqlCommand command = CreateCommand(connection, query, sqlParameters))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
            }
        }

        /// <summary>
        /// For Select Queries.
        /// </summary>
        protected DataTable ExecuteSelectQuery(string query)
        {
            DataTable dataTable = new DataTable();
            DataSet dataSet = new DataSet();

            try
            {
                using (SqlConnection connection = OpenConnection())
                {
                    using (SqlCommand command = CreateCommand(connection, query))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter())
                        {

                            adapter.SelectCommand = command;
                            adapter.Fill(dataSet);
                            dataTable = dataSet.Tables[Zero];
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                HandleSqlException(e);
            }

            return dataTable;
        }

        private void HandleSqlException(SqlException ex)
        {
            // Preserve the original exception as an inner exception
            throw new Exception(DatabaseErrorMessage, ex);
        }

        private SqlCommand CreateCommand(SqlConnection connection, string query, SqlParameter[] parameters = null)
        {
            SqlCommand command = new SqlCommand(query, connection);
            
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            return command;
        }
    }
}

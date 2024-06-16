using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DAL
{
    // This class is written by Sia Iurashchuk
    public abstract class BaseDao
    {
        private const string ConnectionStringName = "ChapeauDatabase";
        private const string DatabaseErrorMessage = "Database operation failed.";
        public static event Action NetworkExceptionOccurredDao;

        private SqlConnection OpenConnection()
        {
            SqlConnection connection = new(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
            connection.Open();
            return connection;
        }

        protected List<T> ReadTable<T>(DataTable dataTable, Func<DataRow, T> readRow)
        {
            List<T> list = new();

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
            catch
            {
                NetworkExceptionHandler();
            }
        }

        /// <summary>
        /// For Select Queries.
        /// </summary>
        protected DataTable ExecuteSelectQuery(string query, out bool error, params SqlParameter[] sqlParameters)
        {
            DataTable dataTable = new();
            DataSet dataSet = new();
            error = false;

            try
            {
                using (SqlConnection connection = OpenConnection())
                {
                    using (SqlCommand command = CreateCommand(connection, query, sqlParameters))
                    {
                        using (SqlDataAdapter adapter = new())
                        {

                            adapter.SelectCommand = command;
                            adapter.Fill(dataSet);
                            dataTable = dataSet.Tables[0];
                        }
                    }
                }
            }
            catch
            {
                NetworkExceptionHandler();
                error = true;
            }

            return dataTable;
        }

        private void NetworkExceptionHandler()
        {
            NetworkExceptionOccurredDao?.Invoke();
        }

        private SqlCommand CreateCommand(SqlConnection connection, string query, params SqlParameter[] sqlParameters)
        {
            SqlCommand command = new(query, connection);
            command.Parameters.AddRange(sqlParameters);
            return command;
        }

        protected T GetByIntParameters<T>(string query, Func<DataRow, T> readRow, Dictionary<string, int> parameters)
        {
            List<T> list = GetAllByIntParameters(query, readRow, parameters, out bool nError);

            if (nError)
                return list.FirstOrDefault();
            return list.First();
        }

        protected List<T> GetAllByIntParameters<T>(string query, Func<DataRow, T> readRow, Dictionary<string, int> parameters, out bool Nerror)
        {
            SqlParameter[] sqlParameters = CreateSqlParameters(parameters);
            DataTable dataTable = ExecuteSelectQuery(query, out bool error, sqlParameters);
            Nerror = error;

            return ReadTable(dataTable, readRow);
        }

        protected List<T> GetAll<T>(string query, Func<DataRow, T> readRow, params SqlParameter[] sqlParameters)
        {
            DataTable dataTable = ExecuteSelectQuery(query, out bool error, sqlParameters);
            return ReadTable(dataTable, readRow);
        }
        
        private SqlParameter[] CreateSqlParameters(Dictionary<string, int> parameters)
        {
            return parameters.Select(p => new SqlParameter(p.Key, p.Value)).ToArray();
        }
    }
}
﻿using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DAL
{
    // This class is written by Sia Iurashchuk
    public abstract class BaseDao
    {
        private const string ConnectionStringName = "ChapeauDatabase";
        private const string DatabaseErrorMessage = "Database operation failed.";
        private const int Zero = 0;

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
            catch (SqlException ex)
            {
                HandleSqlException(ex);
            }
        }

        /// <summary>
        /// For Select Queries.
        /// </summary>
        protected DataTable ExecuteSelectQuery(string query, params SqlParameter[] sqlParameters)
        {
            DataTable dataTable = new();
            DataSet dataSet = new();

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

        private SqlCommand CreateCommand(SqlConnection connection, string query, params SqlParameter[] parameters)
        {
            SqlCommand command = new(query, connection);
            command.Parameters.AddRange(parameters);
            return command;
        }

        protected T GetById<T>(string query, Func<DataRow, T> readRow, Dictionary<string, int> parameters)
        {
            SqlParameter[] sqlParameters = CreateSqlParameters(parameters);
            DataTable dataTable = ExecuteSelectQuery(query, sqlParameters);
            return ReadTable(dataTable, readRow).FirstOrDefault();
        }

        private SqlParameter[] CreateSqlParameters(Dictionary<string, int> parameters)
        {
            return parameters.Select(p => new SqlParameter(p.Key, p.Value)).ToArray();
        }
    }
}

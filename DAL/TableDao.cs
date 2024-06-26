﻿using Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class TableDao : BaseDao
    {
        private const string QueryGetAllTables = $"SELECT {ColumnTableId}, {ColumnHostId}, {ColumnOccupied}, {ColumnTableNumber} FROM [table]";
        private const string QueryGetTableById = $"{QueryGetAllTables} WHERE {ColumnTableId} = {ParameterNameTableId}";
        private const string QueryUpdateTableStatus = $"UPDATE [table] SET {ColumnOccupied} = {ParameterNameOccupied} WHERE {ColumnTableId} = {ParameterNameTableId}";

        private const string ColumnTableId = "table_id";
        private const string ColumnHostId = "host";
        private const string ColumnOccupied = "occupied";
        private const string ColumnTableNumber = "table_number";

        private const string ParameterNameTableId = "@tableId";
        private const string ParameterNameOccupied = "@occupied";

        private EmployeeDao employeeDao = new();

        public List<Table> GetAllTables()
        {
            return GetAll(QueryGetAllTables, ReadRow);
        }

        public Table GetTableById(int tableId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameTableId, tableId }
            };

            return GetByIntParameters(QueryGetTableById, ReadRow, parameters);
        }

        public void UpdateTableStatus(Table table)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new(ParameterNameOccupied, table.Occupied),
                new(ParameterNameTableId, table.DatabaseId)
            };

            ExecuteEditQuery(QueryUpdateTableStatus, parameters);
        }

        private Table ReadRow(DataRow dr)
        {
            int id = (int)dr[ColumnTableId];
            Employee host = employeeDao.GetEmployeeById((int)dr[ColumnHostId]);
            bool occupied = (bool)dr[ColumnOccupied];
            int tableNumber = (int)dr[ColumnTableNumber];

            return new(id, host, occupied, tableNumber);
        }
    }
}
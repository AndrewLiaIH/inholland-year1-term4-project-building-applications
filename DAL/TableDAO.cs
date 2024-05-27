using Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class TableDao : BaseDao
    {
        private const string QueryGetAllTables = $"SELECT {ColumnTableId}, {ColumnHostId}, {ColumnOccupied}, {ColumnTableNumber} FROM [table]";
        private const string QueryGetTableById = $"{QueryGetAllTables} WHERE {ColumnTableId} = {ParameterNameTableId}";

        private const string ColumnTableId = "table_id";
        private const string ColumnHostId = "host";
        private const string ColumnOccupied = "occupied";
        private const string ColumnTableNumber = "table_number";

        private const string ParameterNameTableId = "@tableId";

        private EmployeeDao employeeDao = new();

        public List<Table> GetAllTables()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllTables, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Table GetTableById(int tableId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameTableId, tableId }
            };

            return GetById(QueryGetTableById, ReadRow, parameters);
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
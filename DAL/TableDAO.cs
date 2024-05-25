using Model;
using System.Data;
using System.Data.SqlClient;

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

        public Table GetTableById(uint tableId)
        {
            Dictionary<string, uint> parameters = new()
            {
                { ParameterNameTableId, tableId }
            };

            return GetById(QueryGetTableById, ReadRow, parameters);
        }

        private Table ReadRow(DataRow dr)
        {
            uint id = (uint)dr[ColumnTableId];
            Employee host = employeeDao.GetEmployeeById((uint)dr[ColumnHostId]);
            bool occupied = (bool)dr[ColumnOccupied];
            uint tableNumber = (uint)dr[ColumnTableNumber];

            return new(id, host, occupied, tableNumber);
        }
    }
}
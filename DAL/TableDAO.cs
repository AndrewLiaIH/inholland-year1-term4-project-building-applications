using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class TableDao : BaseDao
    {
        EmployeeDao employeeDao;

        const string QueryGetAllTables = $"SELECT {ColumnTableId}, {ColumnHostId}, {ColumnOccupied}, {ColumnTableNumber} FROM [table]";
        const string QueryGetTableById = $"{QueryGetAllTables} WHERE {ColumnTableId} = {ParameterNameTableId}";

        const string ColumnTableId = "table_id";
        const string ColumnHostId = "host";
        const string ColumnOccupied = "occupied";
        const string ColumnTableNumber = "table_number";

        const string ParameterNameTableId = "@TableId";

        public List<Table> GetAllTables()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllTables, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Table GetTableById(uint tableId)
        {
            Dictionary<string, uint> parameters = new Dictionary<string, uint>()
            {
                { ParameterNameTableId, tableId }
            };

            return GetById(QueryGetTableById, ReadRow, parameters);
        }

        private Table ReadRow(DataRow dr)
        {
            uint id = (uint)dr[ColumnTableId];
            uint hostId = (uint)dr[ColumnHostId];
            bool occupied = (bool)dr[ColumnOccupied];
            uint tableNumber = (uint)dr[ColumnTableNumber];

            return new Table(id, employeeDao.GetEmployeeById(hostId), occupied, tableNumber);
        }
    }
}

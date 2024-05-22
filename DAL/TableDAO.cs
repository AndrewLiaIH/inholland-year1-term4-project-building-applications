using Model;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class TableDAO
    {
        EmployeeDAO employeeDAO;
        const string QueryGetAllTables = $"SELECT {ColumnTableID}, {ColumnHostId}, {ColumnOccupied}, {ColumnTableNumber} FROM [table]";

        const string ColumnTableID = "table_id";
        const string ColumnHostId = "host";
        const string ColumnOccupied = "occupied";
        const string ColumnTableNumber = "table_number";

        public List<Table> GetAllTables()
        {
            SqlParameter[] sqlParameters = new SqlParameter[Zero];
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllTables, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Table GetTableById(uint id)
        {

        }

        private Table ReadRow(DataRow dr)
        {
            uint id = (uint)dr[ColumnTableID];
            uint hostId= (uint)dr[ColumnHostId];
            bool occupied = (bool)dr[ColumnOccupied];
            uint tableNumber = (uint)dr[ColumnTableNumber];
            Employee host = employeeDAO.GetEmployeeById(hostId);

            return new Table(id, host, occupied, tableNumber);
        }
    }
}

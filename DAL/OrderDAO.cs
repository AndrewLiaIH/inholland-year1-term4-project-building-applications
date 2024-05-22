using Model;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class OrderDAO
    {
        TableDAO tableDAO;
        EmployeeDAO employeeDAO;

        const string QueryGetAllOrders = $"SELECT {ColumnOrderID}, {ColumnTableId}, {ColumnPlacedById}, {ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice} FROM order";
        const string ColumnOrderID = "order_id";
        const string ColumnTableId = "table_number";
        const string ColumnPlacedById = "placed_by";
        const string ColumnOrderNumber = "order_number";
        const string ColumnServingNumber = "serving_number";
        const string ColumnFinished = "finished";
        const string ColumnTotalPrice = "total_price";

        public List<Order> GetAllOrders()
        {
            SqlParameter[] sqlParameters = new SqlParameter[Zero];
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllOrders, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        private Order ReadRow(DataRow dr)
        {
            uint id = (uint)dr[ColumnOrderID];
            uint tableId = (uint)dr[ColumnTableId];
            uint employeeId = (uint)dr[ColumnPlacedById]; 
            uint orderNumber = (uint)dr[ColumnOrderNumber];
            uint servingNumber = (uint)dr[ColumnServingNumber];
            bool finished = (bool)dr[ColumnFinished];
            decimal totalPrice = (decimal)dr[ColumnTotalPrice];
            Table table = tableDAO.GetTableById(tableId);
            Employee employee = employeeDAO.GetEmployeeById(employeeId);

            return new Order(id, table, employee, orderNumber, servingNumber, finished, totalPrice);
        }
    }
}

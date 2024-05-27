using Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class OrderDao : BaseDao
    {
        private const string QueryGetAllOrders = $"SELECT {ColumnOrderId}, {ColumnTableId}, {ColumnPlacedById}, {ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice} FROM order";
        private const string QueryGetOrderById = $"{QueryGetAllOrders} WHERE {ColumnOrderId} = {ParameterNameOrderId}";

        private const string ColumnOrderId = "order_id";
        private const string ColumnTableId = "table_number";
        private const string ColumnPlacedById = "placed_by";
        private const string ColumnOrderNumber = "order_number";
        private const string ColumnServingNumber = "serving_number";
        private const string ColumnFinished = "finished";
        private const string ColumnTotalPrice = "total_price";

        private const string ParameterNameOrderId = "@orderId";

        private TableDao tableDao = new();
        private EmployeeDao employeeDao = new();

        public List<Order> GetAllOrders()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllOrders, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Order GetOrderById(int orderId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderId, orderId }
            };

            return GetById(QueryGetOrderById, ReadRow, parameters);
        }

        private Order ReadRow(DataRow dr)
        {
            int id = (int)dr[ColumnOrderId];
            Table table = tableDao.GetTableById((int)dr[ColumnTableId]);
            Employee employee = employeeDao.GetEmployeeById((int)dr[ColumnPlacedById]);
            int orderNumber = (int)dr[ColumnOrderNumber];
            int servingNumber = (int)dr[ColumnServingNumber];
            bool finished = (bool)dr[ColumnFinished];
            decimal totalPrice = (decimal)dr[ColumnTotalPrice];

            return new(id, table, employee, orderNumber, servingNumber, finished, totalPrice);
        }
    }
}
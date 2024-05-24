using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OrderDao : BaseDao
    {
        TableDao tableDao;
        EmployeeDao employeeDao;

        const string QueryGetAllOrders = $"SELECT {ColumnOrderId}, {ColumnTableId}, {ColumnPlacedById}, {ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice} FROM order";
        const string QueryGetOrderById = $"{QueryGetAllOrders} WHERE {ColumnOrderId} = {ParameterNameOrderId}";

        const string ColumnOrderId = "order_id";
        const string ColumnTableId = "table_number";
        const string ColumnPlacedById = "placed_by";
        const string ColumnOrderNumber = "order_number";
        const string ColumnServingNumber = "serving_number";
        const string ColumnFinished = "finished";
        const string ColumnTotalPrice = "total_price";

        const string ParameterNameOrderId = "@OrderId";

        public List<Order> GetAllOrders()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllOrders, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Order GetOrderById(uint orderId)
        {
            Dictionary<string, uint> parameters = new()
            {
                { ParameterNameOrderId, orderId }
            };

            return GetById(QueryGetOrderById, ReadRow, parameters);
        }

        private Order ReadRow(DataRow dr)
        {
            uint id = (uint)dr[ColumnOrderId];
            Table table = tableDao.GetTableById((uint)dr[ColumnTableId]);
            Employee employee = employeeDao.GetEmployeeById((uint)dr[ColumnPlacedById]);
            uint orderNumber = (uint)dr[ColumnOrderNumber];
            uint servingNumber = (uint)dr[ColumnServingNumber];
            bool finished = (bool)dr[ColumnFinished];
            decimal totalPrice = (decimal)dr[ColumnTotalPrice];

            return new Order(id, table, employee, orderNumber, servingNumber, finished, totalPrice);
        }
    }
}
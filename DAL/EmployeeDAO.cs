using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class EmployeeDao : BaseDao
    {
        private const string QueryGetAllEmployees = $"SELECT {ColumnEmployeeId}, {ColumnEmployeeNumber}, {ColumnLogin}, {ColumnPassword}, {ColumnFirstName}, {ColumnLastName}, {ColumnEmail}, {ColumnPhoneNumber}, {ColumnEmployeeType} FROM employee";
        private const string QueryGetEmployeeById = $"{QueryGetAllEmployees} WHERE {ColumnEmployeeId} = {ParameterNameEmployeeId}";

        private const string ColumnEmployeeId = "employee_id";
        private const string ColumnEmployeeNumber = "employee_number";
        private const string ColumnLogin = "login";
        private const string ColumnPassword = "password";
        private const string ColumnFirstName = "first_name";
        private const string ColumnLastName = "last_name";
        private const string ColumnEmail = "email";
        private const string ColumnPhoneNumber = "phone_number";
        private const string ColumnEmployeeType = "occupation";

        private const string ParameterNameEmployeeId = "@employeeId";

        private const string EmployeeErrorMessage = "Unkown employee type.";

        public List<Employee> GetAllEmployees()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllEmployees, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Employee GetEmployeeById(uint employeeId)
        {
            Dictionary<string, uint> parameters = new()
            {
                {ParameterNameEmployeeId, employeeId}
            };

            return GetById(QueryGetEmployeeById, ReadRow, parameters);
        }

        private Employee ReadRow(DataRow dr)
        {
            uint id = (uint)dr[ColumnEmployeeId];
            uint employeeNumber = (uint)dr[ColumnEmployeeNumber];
            uint login = (uint)dr[ColumnLogin];
            uint password = (uint)dr[ColumnPassword];
            string firstName = (string)dr[ColumnFirstName];
            string lastName = (string)dr[ColumnLastName];
            string email = (string)dr[ColumnEmail];
            string phoneNumber = (string)dr[ColumnPhoneNumber];
            EmployeeType employeeType = ConvertToEnum((string)dr[ColumnEmployeeType]);

            return new(id, employeeNumber, login, password, firstName, lastName, email, phoneNumber, employeeType);
        }

        private EmployeeType ConvertToEnum(string employeeType)
        {
            return employeeType switch
            {
                "waiter" => EmployeeType.Waiter,
                "waitress" => EmployeeType.Waiter,
                "chef" => EmployeeType.Chef,
                "bartender" => EmployeeType.Bartender,
                "manager" => EmployeeType.Manager,
                _ => throw new ArgumentException(EmployeeErrorMessage)
            };
        }
    }
}
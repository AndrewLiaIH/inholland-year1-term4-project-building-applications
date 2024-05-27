using Model;
using System.Data;
using Microsoft.Data.SqlClient;

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

        public List<Employee> GetAllEmployees()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllEmployees, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Employee GetEmployeeById(int employeeId)
        {
            Dictionary<string, int> parameters = new()
            {
                {ParameterNameEmployeeId, employeeId}
            };

            return GetById(QueryGetEmployeeById, ReadRow, parameters);
        }

        private Employee ReadRow(DataRow dr)
        {
            int id = (int)dr[ColumnEmployeeId];
            int employeeNumber = (int)dr[ColumnEmployeeNumber];
            int login = (int)dr[ColumnLogin];
            int password = (int)dr[ColumnPassword];
            string firstName = (string)dr[ColumnFirstName];
            string lastName = (string)dr[ColumnLastName];
            string email = (string)dr[ColumnEmail];
            string phoneNumber = (string)dr[ColumnPhoneNumber];
            bool parsedEmployeeType = Enum.TryParse((string)dr[ColumnEmployeeType], out EmployeeType employeeType);

            return new(id, employeeNumber, login, password, firstName, lastName, email, phoneNumber, employeeType);
        }
    }
}
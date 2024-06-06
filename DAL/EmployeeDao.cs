using Microsoft.Data.SqlClient;
using Model;
using System.Data;

namespace DAL
{
    public class EmployeeDao : BaseDao
    {
        private const string QueryGetAllEmployees = $"SELECT {ColumnEmployeeId}, {ColumnEmployeeNumber}, {ColumnLogin}, {ColumnPassword}, {ColumnFirstName}, {ColumnLastName}, {ColumnEmail}, {ColumnPhoneNumber}, {ColumnEmployeeType} FROM employee";
        private const string QueryGetEmployeeById = $"{QueryGetAllEmployees} WHERE {ColumnEmployeeId} = {ParameterNameEmployeeId}";
        private const string QueryGetEmployeeByLoginAndPassword = $"{QueryGetAllEmployees} WHERE {ColumnLogin} = {ParameterNameLogin} AND {ColumnPassword} = {ParameterNamePassword}";

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
        private const string ParameterNameLogin = "@login";
        private const string ParameterNamePassword = "@password";

        public List<Employee> GetAllEmployees()
        {
            return GetAll(QueryGetAllEmployees, ReadRow);
        }

        public Employee GetEmployeeById(int employeeId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameEmployeeId, employeeId }
            };

            return GetByIntParameters(QueryGetEmployeeById, ReadRow, parameters);
        }

        public Employee GetEmployeeByLoginAndPassword(int employeeLogin, string password)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new(ParameterNameLogin, employeeLogin),
                new(ParameterNamePassword, password)
            };

            DataTable dataTable = ExecuteSelectQuery(QueryGetEmployeeByLoginAndPassword, sqlParameters);
            return ReadTable(dataTable, ReadRow).FirstOrDefault();
        }

        private Employee ReadRow(DataRow dr)
        {
            int id = (int)dr[ColumnEmployeeId];
            int employeeNumber = (int)dr[ColumnEmployeeNumber];
            int? login = dr[ColumnLogin] as int?;
            string? firstName = dr[ColumnFirstName] as string;
            string? lastName = dr[ColumnLastName] as string;
            string? email = dr[ColumnEmail] as string;
            string? phoneNumber = dr[ColumnPhoneNumber] as string;
            bool parsedEmployeeType = Enum.TryParse((string)dr[ColumnEmployeeType], out EmployeeType employeeType);
            string? password = dr[ColumnPassword] as string;

            return new(id, employeeNumber, login, password, firstName, lastName, email, phoneNumber, employeeType);
        }
    }
}
using System.Data;
using System.Data.SqlClient;
using Model;

namespace DAL
{
    public class EmployeeDao : BaseDao
    {
        const string QueryGetAllEmployees = $"SELECT {ColumnEmployeeId}, {ColumnEmployeeNumber}, {ColumnLogin}, {ColumnPassword}, {ColumnFirstName}, {ColumnLastName}, {ColumnEmail}, {ColumnPhoneNumber}, {ColumnEmployeeType} FROM employee;";

        const string ColumnEmployeeId = "employee_id";
        const string ColumnEmployeeNumber = "employee_number";
        const string ColumnLogin = "login";
        const string ColumnPassword = "password";
        const string ColumnFirstName = "first_name";
        const string ColumnLastName = "last_name";
        const string ColumnEmail = "email";
        const string ColumnPhoneNumber = "phone_number";
        const string ColumnEmployeeType = "occupation";

        public List<Employee> GetAllEmployees()
        {
            SqlParameter[] sqlParameters = new SqlParameter[Zero];
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllEmployees, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Employee GetEmployeeById(uint id) 
        {

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
            string employeeType = (string)dr[ColumnEmployeeType];

            EmployeeType type = employeeType switch
            {
                "waiter" => EmployeeType.Waiter,
                "waitress" => EmployeeType.Waiter,
                "chef" => EmployeeType.Chef,
                "bartender" => EmployeeType.Bartender,
                "manager" => EmployeeType.Manager,
                _ => throw new ArgumentException("Unkown employee type")
            };

            return new Employee(id, employeeNumber, login, password, firstName, lastName, email, phoneNumber, type);
        }
    }
}

using System.Data;
using System.Data.SqlClient;
using Model;

namespace DAL
{
    public class EmployeeDAO : BaseDAO
    {
        const string QueryGetAllEmployees = $"SELECT {ColumnEmployeeID}, {ColumnEmployeeNumber}, {ColumnLogin}, {ColumnPassword}, {ColumnFirstName}, {ColumnLastName}, {ColumnEmail}, {ColumnPhoneNumber}, {ColumnEmployeeType} FROM employee;";

        const string ColumnEmployeeID = "employee_id";
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
            uint id = (uint)dr[ColumnEmployeeID];
            uint employeeNumber = (uint)dr[ColumnEmployeeNumber];
            uint login = (uint)dr[ColumnLogin];
            uint password = (uint)dr[ColumnPassword];
            string firstName = (string)dr[ColumnFirstName];
            string lastName = (string)dr[ColumnLastName];
            string email = (string)dr[ColumnEmail];
            string phoneNumber = (string)dr[ColumnPhoneNumber];
            string employeeType = (string)dr[ColumnEmployeeType];
            EmployeeType type;            

            switch (employeeType)
            {
                case "waiter":
                    type = EmployeeType.Waiter;
                    break;
                case "waitress":
                    type = EmployeeType.Waiter;
                    break;
                case "chef":
                    type = EmployeeType.Chef;
                    break;
                case "bartender":
                    type = EmployeeType.Bartender;
                    break;
                default:
                    type = EmployeeType.Manager;
                    break;
            }

            return new Employee(id, employeeNumber, login, password, firstName, lastName, email, phoneNumber, type);
        }
    }
}

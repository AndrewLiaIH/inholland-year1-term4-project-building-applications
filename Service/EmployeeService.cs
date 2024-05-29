using DAL;
using Model;
using System.Security.Cryptography;
using System.Text;

namespace Service
{
    // This class was written by Andrew Lia
    public class EmployeeService
    {
        private EmployeeDao employeeDao = new();

        public List<Employee> GetAllEmployees()
        {
            return employeeDao.GetAllEmployees();
        }

        public Employee GetEmployeeById(int id)
        {
            return employeeDao.GetEmployeeById(id);
        }

        public Employee GetEmployeeByLoginAndPassword(int login, string password)
        {
            string hashedPassword = HashPassword(password);
            return employeeDao.GetEmployeeByLoginAndPassword(login, hashedPassword);
        }

        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
using DAL;
using Model;

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

        public Employee GetEmployeeById(uint id)
        {
            return employeeDao.GetEmployeeById(id);
        }
    }
}

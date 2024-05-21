namespace Model
{
    public class Employee
    {
        public uint EmployeeNumber { get; set; }
        public uint Login {  get; set; }
        public uint Password { get; set; } //???
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public EmployeeType Type { get; set; }

        public Employee(uint employeeNumber, uint login, uint password, string firstName, string lastName, string email, string phoneNumber, EmployeeType type)
        {
            EmployeeNumber = employeeNumber;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Type = type;
        }
    }
}

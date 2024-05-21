namespace Model
{
    public class Employee
    {
        public uint EmployeeNumber { get; private set; }
        public uint Login {  get; private set; }
        public uint Password { get; private set; } //Encryption?
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public EmployeeType Type { get; private set; }

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

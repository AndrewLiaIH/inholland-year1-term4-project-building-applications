namespace Model
{
    public class Employee
    {
        public int DatabaseId { get; private set; }
        public int EmployeeNumber { get; private set; }
        public int? Login { get; private set; }
        public int? Password { get; private set; } //Encryption?
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public EmployeeType Type { get; private set; }

        public Employee(int databaseId, int employeeNumber, int login, int password, string firstName, string lastName, string email, string phoneNumber, EmployeeType type)
        {
            DatabaseId = databaseId;
            EmployeeNumber = employeeNumber;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Type = type;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, {Type}";
        }
    }
}

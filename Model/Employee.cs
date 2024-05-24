namespace Model
{
    public class Employee
    {
        public uint DatabaseId { get; private set; }
        public uint EmployeeNumber { get; private set; }
        public uint? Login { get; private set; }
        public uint? Password { get; private set; } //Encryption?
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public EmployeeType Type { get; private set; }

        public Employee(uint databaseId, uint employeeNumber, uint login, uint password, string firstName, string lastName, string email, string phoneNumber, EmployeeType type)
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

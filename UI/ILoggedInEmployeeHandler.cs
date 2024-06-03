using Model;

namespace UI
{
    internal interface ILoggedInEmployeeHandler
    {
        void SetLoggedInEmployee(Employee employee);
        UserControlHeader UserControlHeader { get; }
    }
}

namespace ViewModel
{
    public class CommandLogin : CommandBase
    {
        private readonly NavigationService navigationService;

        public CommandLogin(NavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            // Write your login logic here

            navigationService.Navigate();
        }
    }
}

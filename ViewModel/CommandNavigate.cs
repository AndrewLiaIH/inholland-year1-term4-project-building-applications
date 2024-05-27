namespace ViewModel
{
    public class CommandNavigate : CommandBase
    {
        private NavigationService navigationService;

        public CommandNavigate(NavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            navigationService.Navigate();
        }
    }
}

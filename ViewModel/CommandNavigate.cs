namespace ViewModel
{
    internal class CommandNavigate : CommandBase
    {
        private readonly NavigationService navigationService;

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

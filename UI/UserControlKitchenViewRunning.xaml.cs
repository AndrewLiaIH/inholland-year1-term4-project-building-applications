using Model;
using Service;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlKitchenView.xaml
    /// </summary>
    public partial class UserControlKitchenViewRunning : UserControl
    {
        private OrderService orderService = new();
        public List<Order> Orders { get; private set; }
        private DispatcherTimer timer;

        public UserControlKitchenViewRunning()
        {
            InitializeComponent();
            LoadOrders();
            InitializeTimer();
            DataContext = this;
        }

        private void LoadOrders()
        {
            Orders = orderService.GetAllRunningOrders();
        }

        private void InitializeTimer()
        {
            timer = new()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var item in OrdersItemsControl.Items)
            {
                ContentPresenter container = OrdersItemsControl.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;
                TextBlock textBlock = container?.ContentTemplate.FindName("RunningTimeTextBlock", container) as TextBlock;
                Rectangle headerBackground = (Rectangle)container?.ContentTemplate.FindName("CardHeaderBackground", container);

                if (textBlock != null && item is Order order)
                {
                    TimeSpan runningTime = order.OrderItems[0].RunningTime;
                    textBlock.Text = $"{runningTime.Minutes:D2}:{runningTime.Seconds:D2}";

                    switch (runningTime.Minutes)
                    {
                        case > 10:
                            headerBackground.Fill = (SolidColorBrush)FindResource("Color6");
                            break;
                        case > 5:
                            headerBackground.Fill = (SolidColorBrush)FindResource("Color8");
                            break;
                        default:
                            headerBackground.Fill = (SolidColorBrush)FindResource("Color9");
                            break;
                    }
                }
            }
        }
    }
}

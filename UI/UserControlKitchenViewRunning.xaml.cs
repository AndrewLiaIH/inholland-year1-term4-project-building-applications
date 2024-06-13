using Model;
using Service;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using Microsoft.IdentityModel.Tokens;

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
            Orders = orderService.GetAllWaitingAndPreparingOrders();
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
            foreach (Order order in OrdersItemsControl.Items)
            {
                ContentPresenter container = OrdersItemsControl.ItemContainerGenerator.ContainerFromItem(order) as ContentPresenter;
                TextBlock textBlock = container?.ContentTemplate.FindName("RunningTimeTextBlock", container) as TextBlock;
                Rectangle headerBackground = (Rectangle)container?.ContentTemplate.FindName("CardHeaderBackground", container);

                if (textBlock != null)
                {
                    TimeSpan runningTime = order.OrderItems.IsNullOrEmpty() ? TimeSpan.Zero : order.OrderItems[0].RunningTime;
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

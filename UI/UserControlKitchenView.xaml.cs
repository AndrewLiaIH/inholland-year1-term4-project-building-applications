﻿using DAL;
using Model;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlKitchenView.xaml
    /// </summary>
    public partial class UserControlKitchenView : UserControl, ILoggedInEmployeeHandler
    {
        public List<Folder> FoldersKitchen;
        public UserControlHeader UserControlHeader => userControlHeader;
        public UserControlKitchenViewRunning userControlKitchenViewRunning;
        public UserControlKitchenViewFinished userControlKitchenViewFinished;
        private UserControlNetworkError userControlNetworkError;

        private OrderService orderService = new();

        public UserControlKitchenView()
        {
            InitializeComponent();

            FoldersKitchen = new()
            {
                new Folder("Running", ShowKitchenViewRunning),
                new Folder("Finished", ShowKitchenViewFinished)
            };

            userControlHeader.Folders = FoldersKitchen;
            userControlHeader.SelectedFolder = FoldersKitchen.First();
            orderService.NetworkExceptionOccurred += NetworkExceptionOccurred;
        }

        private void NetworkExceptionOccurred()
        {
            Dispatcher.Invoke(() =>
            {
                ShowNetworkErrorView();
                orderService.RunningOrdersChanged += UpdateOrders;
            });
        }

        private void UpdateOrders()
        {
            Task.Run(async () =>
            {
                orderService.RunningOrdersChanged -= UpdateOrders;

                if (orderService.ConnectionAvalible<OrderDao>())
                {
                    await Task.Delay(6000);
                    
                    Dispatcher.Invoke(() =>
                    {
                        ShowKitchenViewRunning();
                    });
                }
            });
        }

        public void SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }

        private void ShowKitchenViewRunning()
        {
            userControlKitchenViewRunning = new();

            KitchenViewContentControl.Content = userControlKitchenViewRunning;

            if (userControlHeader.LoggedInEmployee != null)
            {
                bool forKitchen = userControlHeader.LoggedInEmployee.Type == EmployeeType.Chef ? true : false;
                userControlKitchenViewRunning.LoadOrders(forKitchen);
            }
        }

        private void ShowKitchenViewFinished()
        {
            userControlKitchenViewFinished = new();

            KitchenViewContentControl.Content = userControlKitchenViewFinished;

            bool forKitchen = userControlHeader.LoggedInEmployee.Type == EmployeeType.Chef ? true : false;
            userControlKitchenViewFinished.LoadOrders(forKitchen);
        }

        private void ShowNetworkErrorView()
        {
            userControlNetworkError ??= new();
            KitchenViewContentControl.Content = userControlNetworkError;
        }
    }
}

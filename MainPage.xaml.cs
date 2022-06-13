using BudgetPlaning.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace BudgetPlaning
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.Balance.Text = "Баланс:";

            ShowBalance();

            var t = ApplicationView.GetForCurrentView().TitleBar;
            t.BackgroundColor = Colors.LightGray;
            t.ForegroundColor = Colors.White;
            t.ButtonBackgroundColor = Colors.LightGray;
            t.ButtonForegroundColor = Colors.White;

            foreach (NavigationViewItemBase item in NavigationViewControl.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Invoice")
                {
                    NavigationViewControl.SelectedItem = item;
                    break;
                }
            }
        }

        private async void ShowBalance()
        {
            try
            {
                var balance = Controllers.Balance.GetBalance();
                if (balance >= 0)
                    this.BalanceSum.Text = $"{Controllers.Balance.GetBalance().ToString("C", CultureInfo.GetCultureInfo("en-US"))}";
                else
                    this.BalanceSum.Text = $"{Controllers.Balance.GetBalance().ToString("C", CultureInfo.GetCultureInfo("en-US")).Replace("(", "").Replace(")", "").Insert(0, "-")}";
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }

        private void NavigationViewControl_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "Invoice":
                    Frame.Navigate(typeof(MainPage), null, new SuppressNavigationTransitionInfo());
                    break;

                case "NewOperation":
                    Frame.Navigate(typeof(NewOperationPage), null, new SuppressNavigationTransitionInfo());
                    break;

                case "History":
                    Frame.Navigate(typeof(HistoryPage), null, new SuppressNavigationTransitionInfo());
                    break;
                case "Statistics":
                    Frame.Navigate(typeof(StatisticsPage), null, new SuppressNavigationTransitionInfo());
                    break;
            }
        }

        private void NavigationViewControl_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
            NavigationViewControl_Navigate(item as NavigationViewItem);
        }
    }
}

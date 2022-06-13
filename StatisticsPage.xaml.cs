using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using BudgetPlaning.View;
using BudgetPlaning.Controllers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using BudgetPlaning.Models;
using Windows.UI.Popups;
using System.Threading.Tasks;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace BudgetPlaning
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            this.InitializeComponent();

            Income1.Text = "Доход:";
            Income2.Text = "Доход:";
            Income3.Text = "Доход:";
            Expense1.Text = "Расход:";
            Expense2.Text = "Расход:";
            Expense3.Text = "Расход:";

            foreach (NavigationViewItemBase item in NavigationViewControl.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Statistics")
                {
                    NavigationViewControl.SelectedItem = item;
                    break;
                }
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

        private async void TabBar_PivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            try
            {
                switch ((sender.SelectedItem as PivotItem).Name)
                {
                    case "Week":
                        IncomeSum1.Text = Statistics.GetStatistics("Week").Item1;
                        ExpenseSum1.Text = Statistics.GetStatistics("Week").Item2; break;
                    case "Month":
                        IncomeSum2.Text = Statistics.GetStatistics("Month").Item1;
                        ExpenseSum2.Text = Statistics.GetStatistics("Month").Item2; break;
                    case "Year":
                        IncomeSum3.Text = Statistics.GetStatistics("Year").Item1;
                        ExpenseSum3.Text = Statistics.GetStatistics("Year").Item2; break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }
    }
}

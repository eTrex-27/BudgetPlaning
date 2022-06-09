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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace BudgetPlaning
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            this.InitializeComponent();

            foreach (NavigationViewItemBase item in NavigationViewControl.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "History")
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
            }
        }

        private void NavigationViewControl_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
            NavigationViewControl_Navigate(item as NavigationViewItem);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = ViewGridData.GetRecords();
        }

        private void dataGrid_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.IsReadOnly = true;
            switch(e.Column.Header.ToString())
            {
                case "Date": e.Column.Header = "Дата"; break;
                case "Summ": e.Column.Header = "Сумма"; break;
                case "Type": e.Column.Header = "Тип"; break;
                case "Category": e.Column.Header = "Категория"; break;
                case "Comment": e.Column.Header = "Комментарий"; break;
                default: break;
            }
        }
    }
}

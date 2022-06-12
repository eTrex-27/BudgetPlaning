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
    public sealed partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            this.InitializeComponent();

            ClearHistoryButton.Content = "Очистить историю";

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

        private void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Connection.CleareRecords();
            dataGrid.ItemsSource = ViewGridData.GetRecords();
        }

        private void dataGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            var row = e.Row.GetIndex();
            var column = "";

            switch (e.Column.Header.ToString())
            {
                case "Дата": column = "Date" ; break;
                case "Сумма": column = "Summ"; break;
                case "Тип": column = "Type"; break;
                case "Категория": column = "Category"; break;
                case "Комментарий": column = "Comment"; break;
                default: break;
            }

            UpdateRecord(row, column, ((sender as DataGrid).SelectedItem as Record).GetValueOfField(column));

            dataGrid.ItemsSource = ViewGridData.GetRecords();
        }

        private async void UpdateRecord(int row, string column, string value)
        {
            try
            {
                UpdateRecords.Update(row, column, value);
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }
    }
}

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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace BudgetPlaning
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class NewOperationPage : Page
    {
        public NewOperationPage()
        {
            this.InitializeComponent();

            this.SumOperation.Text = "Сумма операции:";
            this.TypeOperation.Text = "Тип операции:";

            var itemsTypesOperation = new List<string>()
            {
                "Доход",
                "Расход"
            };

            this.TypeOperationField.ItemsSource = itemsTypesOperation;
            this.TypeOperationField.SelectedIndex = 0;

            this.Category.Text = "Категория:";

            this.Comment.Text = "Комментарий:";

            this.ClearButton.Content = "Сбросить";
            this.WriteButton.Content = "Записать";

            foreach (NavigationViewItemBase item in NavigationViewControl.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "NewOperation")
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
    }
}

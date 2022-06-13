using BudgetPlaning.Controllers;
using BudgetPlaning.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class NewOperationPage : Page
    {
        public NewOperationPage()
        {
            this.InitializeComponent();

            this.SumOperation.Text = "Сумма операции:";
            this.TypeOperation.Text = "Тип операции:";

            var itemsTypesOperation = OperationTypes.GetTypes();

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

        private async void WriteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var date = DateTime.Now.ToString("dd.MM.yyyy, HH:mm");
                var summ = SumOperationField.Text;
                var type = TypeOperationField.SelectedValue != null ? TypeOperationField.SelectedValue.ToString() : "";
                var category = CategoryField.SelectedValue != null ? CategoryField.SelectedValue.ToString() : "";
                var comment = CommentField.Text;

                var validate = Validation.ValidateSumField(SumOperationField.Text);

                if (validate.Item1 && Connection.ParseDouble(SumOperationField.Text) != 0.00)
                {
                    HintError.Text = "";
                    SumOperationField.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(66, 00, 00, 00));
                    Connection.AddRecord(date, summ, type, category, comment);
                }
                else if (string.IsNullOrEmpty(SumOperationField.Text))
                {
                    HintError.Text = "Поле обязательно для заполнения";
                    SumOperationField.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                }
                else
                {
                    HintError.Text = "Сумма имеет неправильный формат";
                    SumOperationField.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                }
            }
            catch (Exception ex)
            {
                HintError.Text = "Сумма имеет неправильный формат";
                SumOperationField.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }

        private void TypeOperationField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TypeOperationField.SelectedValue.ToString())
            {
                case "Доход": CategoryField.ItemsSource = new CategoriesIncome().GetCategories(); CategoryField.SelectedIndex = 0; break;
                case "Расход": CategoryField.ItemsSource = new CategoriesExpense().GetCategories(); CategoryField.SelectedIndex = 0; break;
                default: break;
            }
        }

        private void SumOperationField_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (args.IsContentChanging)
            {
                var validate = Validation.ValidateSumField(sender.Text);
                if (!validate.Item1)
                {
                    HintError.Text = validate.Item2;
                    sender.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                }
                else
                {
                    HintError.Text = "";
                    sender.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(66, 00, 00, 00));
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SumOperationField.Text = "";
            CommentField.Text = "";
            CategoryField.SelectedIndex = 0;
            TypeOperationField.SelectedIndex = 0;
        }
    }
}

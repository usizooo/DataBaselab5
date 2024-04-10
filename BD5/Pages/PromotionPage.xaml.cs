using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BD5.Pages
{
    /// <summary>
    /// Interaction logic for PromotionPage.xaml
    /// </summary>
    public partial class PromotionPage : Page
    {
        private ClubEntities context = new ClubEntities();

        public PromotionPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.promotion.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is promotion)
            {
                var _promotion = dataGrid.SelectedItem as promotion;

                nameofpromotionTextBox.Text = _promotion.nameofpromotion;
                descriptionofpromotionTextBox.Text = _promotion.descriptionofpromotion;
                stardateDatePicker.SelectedDate = _promotion.stardate;
                enddateDatePicker.SelectedDate = _promotion.enddate;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            var _promotion = new promotion();

            if (string.IsNullOrWhiteSpace(nameofpromotionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите наименование акции.");
                return;
            }

            _promotion.nameofpromotion = nameofpromotionTextBox.Text;
            _promotion.descriptionofpromotion = descriptionofpromotionTextBox.Text;

            _promotion.stardate = stardateDatePicker.SelectedDate;
            _promotion.enddate = enddateDatePicker.SelectedDate;

            if (!ValidateDates())
                return;

            context.promotion.Add(_promotion);
            context.SaveChanges();

            dataGrid.ItemsSource = context.promotion.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is promotion)
            {
                var _promotion = dataGrid.SelectedItem as promotion;

                if (string.IsNullOrWhiteSpace(nameofpromotionTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите наименование акции.");
                    return;
                }

                _promotion.nameofpromotion = nameofpromotionTextBox.Text;
                _promotion.descriptionofpromotion = descriptionofpromotionTextBox.Text;

                _promotion.stardate = stardateDatePicker.SelectedDate;
                _promotion.enddate = enddateDatePicker.SelectedDate;

                if (!ValidateDates())
                    return;

                context.SaveChanges();

                dataGrid.ItemsSource = context.promotion.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is promotion)
            {
                var selectedPromotion = dataGrid.SelectedItem as promotion;

                context.promotion.Remove(selectedPromotion);
                context.SaveChanges();

                dataGrid.ItemsSource = context.promotion.ToList();
            }
        }

        private bool ValidateDates()
        {
            if (stardateDatePicker.SelectedDate != null && enddateDatePicker.SelectedDate != null)
            {
                if (stardateDatePicker.SelectedDate >= enddateDatePicker.SelectedDate)
                {
                    MessageBox.Show("Дата окончания акции должна быть позже даты начала.");
                    return false;
                }
            }
            return true;
        }
    }
}

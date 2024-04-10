using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BD5.Pages
{
    /// <summary>
    /// Логика взаимодействия для PlacePage.xaml
    /// </summary>
    public partial class PlacePage : Page
    {
        private ClubEntities context = new ClubEntities();
        public PlacePage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.place.ToList();
            devicesComboBox.ItemsSource = context.devices.ToList();
            specificationsComboBox.ItemsSource = context.specifications.ToList();
            employmentstatusComboBox.ItemsSource = context.place.Select(x => x.employmentstatus).Distinct().ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is place)
            {
                var _place = dataGrid.SelectedItem as place;

                nameofcomputerTextBox.Text = _place.nameofcomputer;
                employmentstatusComboBox.SelectedItem = _place.employmentstatus;
                priceTextBox.Text = _place.price.ToString();
                var selectedspecifications = context.specifications.FirstOrDefault(s => s.ID_specifications == _place.specifications_ID);
                specificationsComboBox.SelectedItem = selectedspecifications;
                var selecteddevices = context.devices.FirstOrDefault(d => d.ID_devices == _place.devices_ID);
                devicesComboBox.SelectedItem = selecteddevices;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidInput())
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            place _place = new place();

            _place.nameofcomputer = nameofcomputerTextBox.Text;
            _place.employmentstatus = employmentstatusComboBox.SelectedItem.ToString();
            _place.price = Convert.ToInt32(priceTextBox.Text);
            _place.specifications_ID = (specificationsComboBox.SelectedItem as specifications).ID_specifications;
            _place.devices_ID = (devicesComboBox.SelectedItem as devices).ID_devices;

            context.place.Add(_place);
            context.SaveChanges();
            dataGrid.ItemsSource = context.place.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is place)
            {
                if (!IsValidInput())
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var _place = dataGrid.SelectedItem as place;

                _place.nameofcomputer = nameofcomputerTextBox.Text;
                _place.employmentstatus = employmentstatusComboBox.SelectedItem.ToString();
                _place.price = Convert.ToInt32(priceTextBox.Text);
                _place.specifications_ID = (specificationsComboBox.SelectedItem as specifications).ID_specifications;
                _place.devices_ID = (devicesComboBox.SelectedItem as devices).ID_devices;

                context.SaveChanges();

                dataGrid.ItemsSource = context.place.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is place)
            {
                var _place = dataGrid.SelectedItem as place;
                context.place.Remove(_place);
                context.SaveChanges();
                dataGrid.ItemsSource = context.place.ToList();
            }
        }

        private bool IsValidInput()
        {
            return !string.IsNullOrWhiteSpace(nameofcomputerTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(employmentstatusComboBox.Text) &&
                   !string.IsNullOrWhiteSpace(priceTextBox.Text) &&
                   devicesComboBox.SelectedItem != null &&
                   specificationsComboBox.SelectedItem != null &&
                   IsPositiveInteger(priceTextBox.Text);
        }

        private bool IsPositiveInteger(string text)
        {
            return int.TryParse(text, out int result) && result > 0;
        }
    }
}

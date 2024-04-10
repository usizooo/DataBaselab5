using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BD5.Pages
{
    /// <summary>
    /// Логика взаимодействия для DevicesPage.xaml
    /// </summary>
    public partial class DevicesPage : Page
    {
        ClubEntities context = new ClubEntities();

        public DevicesPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.devices.ToList();
        }


        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is devices)
            {
                var _devices = dataGrid.SelectedItem as devices;

                mouseTextBox.Text = _devices.mouse;
                headphonesTextBox.Text = _devices.headphones;
                keyboardTextBox.Text = _devices.keyboard;
                microphoneTextBox.Text = _devices.microphone;
                screenTextBox.Text = _devices.screen;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            var newDevices = new devices
            {
                mouse = mouseTextBox.Text,
                headphones = headphonesTextBox.Text,
                keyboard = keyboardTextBox.Text,
                microphone = microphoneTextBox.Text,
                screen = screenTextBox.Text
            };

            context.devices.Add(newDevices);
            context.SaveChanges();

            dataGrid.ItemsSource = context.devices.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is devices)
            {
                var selectedDevices = dataGrid.SelectedItem as devices;
                if (!ValidateInput())
                    return;

                selectedDevices.mouse = mouseTextBox.Text;
                selectedDevices.headphones = headphonesTextBox.Text;
                selectedDevices.keyboard = keyboardTextBox.Text;
                selectedDevices.microphone = microphoneTextBox.Text;
                selectedDevices.screen = screenTextBox.Text;

                context.SaveChanges();

                dataGrid.ItemsSource = context.devices.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is devices)
            {
                var selectedDevices = dataGrid.SelectedItem as devices;

                context.devices.Remove(selectedDevices);
                context.SaveChanges();

                dataGrid.ItemsSource = context.devices.ToList();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(mouseTextBox.Text) || string.IsNullOrWhiteSpace(headphonesTextBox.Text) ||
                string.IsNullOrWhiteSpace(keyboardTextBox.Text) || string.IsNullOrWhiteSpace(microphoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(screenTextBox.Text))
            {
                MessageBox.Show("Заполните все поля для устройств!");
                return false;
            }

            return true;
        }
    }
}

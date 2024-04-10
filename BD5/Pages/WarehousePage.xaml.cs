using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BD5.Pages
{
    /// <summary>
    /// Логика взаимодействия для WarehousePage.xaml
    /// </summary>
    public partial class WarehousePage : Page
    {
        ClubEntities context = new ClubEntities();
        public WarehousePage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.warehouse.ToList();
        }


        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is warehouse)
            {
                var _warehouse = dataGrid.SelectedItem as warehouse;

                nameofwarehouseTextBox.Text = _warehouse.nameofwarehouse;
                processorTextBox.Text = _warehouse.processor;
                motherboardTextBox.Text = _warehouse.motherboard;
                computercaseTextBox.Text = _warehouse.computercase;
                videocardTextBox.Text = _warehouse.videocard;
                cpucoolingTextBox.Text = _warehouse.cpucooling;
                ramTextBox.Text = _warehouse.ram;
                datastorageTextBox.Text = _warehouse.datastorage;
                powersuppplyTextBox.Text = _warehouse.powersuppply;
                mouseTextBox.Text = _warehouse.mouse;
                headphonesTextBox.Text = _warehouse.headphones;
                keyboardTextBox.Text = _warehouse.keyboard;
                microphoneTextBox.Text = _warehouse.microphone;
                screenTextBox.Text = _warehouse.screen;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreAllFieldsFilled())
            {
                var _warehouse = new warehouse();

                _warehouse.nameofwarehouse = nameofwarehouseTextBox.Text;
                _warehouse.processor = processorTextBox.Text;
                _warehouse.motherboard = motherboardTextBox.Text;
                _warehouse.computercase = computercaseTextBox.Text;
                _warehouse.videocard = videocardTextBox.Text;
                _warehouse.cpucooling = cpucoolingTextBox.Text;
                _warehouse.ram = ramTextBox.Text;
                _warehouse.datastorage = datastorageTextBox.Text;
                _warehouse.powersuppply = powersuppplyTextBox.Text;
                _warehouse.mouse = mouseTextBox.Text;
                _warehouse.headphones = headphonesTextBox.Text;
                _warehouse.keyboard = keyboardTextBox.Text;
                _warehouse.microphone = microphoneTextBox.Text;
                _warehouse.screen = screenTextBox.Text;

                context.warehouse.Add(_warehouse);
                context.SaveChanges();

                dataGrid.ItemsSource = context.warehouse.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is warehouse)
            {
                if (AreAllFieldsFilled())
                {
                    var _warehouse = dataGrid.SelectedItem as warehouse;

                    _warehouse.nameofwarehouse = nameofwarehouseTextBox.Text;
                    _warehouse.processor = processorTextBox.Text;
                    _warehouse.motherboard = motherboardTextBox.Text;
                    _warehouse.computercase = computercaseTextBox.Text;
                    _warehouse.videocard = videocardTextBox.Text;
                    _warehouse.cpucooling = cpucoolingTextBox.Text;
                    _warehouse.ram = ramTextBox.Text;
                    _warehouse.datastorage = datastorageTextBox.Text;
                    _warehouse.powersuppply = powersuppplyTextBox.Text;
                    _warehouse.mouse = mouseTextBox.Text;
                    _warehouse.headphones = headphonesTextBox.Text;
                    _warehouse.keyboard = keyboardTextBox.Text;
                    _warehouse.microphone = microphoneTextBox.Text;
                    _warehouse.screen = screenTextBox.Text;

                    context.SaveChanges();

                    dataGrid.ItemsSource = context.warehouse.ToList();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is warehouse)
            {
                var selectedWarehouse = dataGrid.SelectedItem as warehouse;

                context.warehouse.Remove(selectedWarehouse);
                context.SaveChanges();

                dataGrid.ItemsSource = context.warehouse.ToList();
            }
        }

        private bool AreAllFieldsFilled()
        {
            return !string.IsNullOrWhiteSpace(nameofwarehouseTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(processorTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(motherboardTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(computercaseTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(videocardTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(cpucoolingTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(ramTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(datastorageTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(powersuppplyTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(mouseTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(headphonesTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(keyboardTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(microphoneTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(screenTextBox.Text);
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BD5.Pages
{
    /// <summary>
    /// Логика взаимодействия для SpecificationsPage.xaml
    /// </summary>
    public partial class SpecificationsPage : Page
    {
        private ClubEntities context = new ClubEntities();

        public SpecificationsPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.specifications.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is specifications)
            {
                var _specifications = dataGrid.SelectedItem as specifications;
                processorTextBox.Text = _specifications.processor;
                motherboardTextBox.Text = _specifications.motherboard;
                computercaseTextBox.Text = _specifications.computercase;
                videocardTextBox.Text = _specifications.videocard;
                cpucoolingTextBox.Text = _specifications.cpucooling;
                ramTextBox.Text = _specifications.ram;
                datastorageTextBox.Text = _specifications.datastorage;
                powersuppplyTextBox.Text = _specifications.powersuppply;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
                return;

            var _specifications = new specifications();
            _specifications.processor = processorTextBox.Text;
            _specifications.motherboard = motherboardTextBox.Text;
            _specifications.computercase = computercaseTextBox.Text;
            _specifications.videocard = videocardTextBox.Text;
            _specifications.cpucooling = cpucoolingTextBox.Text;
            _specifications.ram = ramTextBox.Text;
            _specifications.datastorage = datastorageTextBox.Text;
            _specifications.powersuppply = powersuppplyTextBox.Text;

            context.specifications.Add(_specifications);
            context.SaveChanges();

            dataGrid.ItemsSource = context.specifications.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is specifications)
            {
                if (!ValidateFields())
                    return;

                var _specifications = dataGrid.SelectedItem as specifications;
                _specifications.processor = processorTextBox.Text;
                _specifications.motherboard = motherboardTextBox.Text;
                _specifications.computercase = computercaseTextBox.Text;
                _specifications.videocard = videocardTextBox.Text;
                _specifications.cpucooling = cpucoolingTextBox.Text;
                _specifications.ram = ramTextBox.Text;
                _specifications.datastorage = datastorageTextBox.Text;
                _specifications.powersuppply = powersuppplyTextBox.Text;

                context.SaveChanges();

                dataGrid.ItemsSource = context.specifications.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is specifications)
            {
                var selectedSpecifications = dataGrid.SelectedItem as specifications;
                context.specifications.Remove(selectedSpecifications);
                context.SaveChanges();
                dataGrid.ItemsSource = context.specifications.ToList();
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(processorTextBox.Text) ||
                string.IsNullOrWhiteSpace(motherboardTextBox.Text) ||
                string.IsNullOrWhiteSpace(computercaseTextBox.Text) ||
                string.IsNullOrWhiteSpace(videocardTextBox.Text) ||
                string.IsNullOrWhiteSpace(cpucoolingTextBox.Text) ||
                string.IsNullOrWhiteSpace(ramTextBox.Text) ||
                string.IsNullOrWhiteSpace(datastorageTextBox.Text) ||
                string.IsNullOrWhiteSpace(powersuppplyTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return false;
            }
            return true;
        }
    }
}

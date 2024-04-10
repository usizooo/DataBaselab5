using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BD5.Pages
{
    /// <summary>
    /// Interaction logic for SessionLengthsPage.xaml
    /// </summary>
    public partial class SessionLengthsPage : Page
    {
        private ClubEntities context = new ClubEntities();

        public SessionLengthsPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.sessionlengths.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is sessionlengths)
            {
                var _sessionlengths = dataGrid.SelectedItem as sessionlengths;
                timerateTextBox.Text = _sessionlengths.timerate.ToString();
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            int timerate;
            if (!TryInputTimerate(out timerate))
                return;

            var _sessionlengths = new sessionlengths();
            _sessionlengths.timerate = timerate;

            context.sessionlengths.Add(_sessionlengths);
            context.SaveChanges();

            dataGrid.ItemsSource = context.sessionlengths.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is sessionlengths)
            {
                var _sessionlengths = dataGrid.SelectedItem as sessionlengths;
                int timerate;
                if (!TryInputTimerate(out timerate))
                    return;

                _sessionlengths.timerate = timerate;

                context.SaveChanges();

                dataGrid.ItemsSource = context.sessionlengths.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is sessionlengths)
            {
                var selectedSessionlengths = dataGrid.SelectedItem as sessionlengths;
                context.sessionlengths.Remove(selectedSessionlengths);
                context.SaveChanges();
                dataGrid.ItemsSource = context.sessionlengths.ToList();
            }
        }

        private bool TryInputTimerate(out int timerate)
        {
            if (int.TryParse(timerateTextBox.Text, out timerate))
            {
                if (timerate >= 60 && timerate <= 1440)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Время сессии должно быть не менее 60 минут и не более 1440 минут!");
                    timerateTextBox.Text = string.Empty;
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Введите корректное число для времени сессии.");
                timerateTextBox.Text = string.Empty;
                return false;
            }
        }
    }
}

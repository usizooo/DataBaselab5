using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для BuyersPage.xaml
    /// </summary>
    public partial class BuyersPage : Page
    {
        private ClubEntities context = new ClubEntities();

        public BuyersPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.buyers.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is buyers)
            {
                var _buyers = dataGrid.SelectedItem as buyers;

                nicknameTextBox.Text = _buyers.nickname;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            string nickname;
            if (!TryInputNickname(out nickname))
                return;

            var newBuyer = new buyers
            {
                nickname = nickname
            };

            context.buyers.Add(newBuyer);
            context.SaveChanges();

            dataGrid.ItemsSource = context.buyers.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is buyers)
            {
                var selectedBuyer = dataGrid.SelectedItem as buyers;
                string nickname;
                if (!TryInputNickname(out nickname))
                    return;

                selectedBuyer.nickname = nickname;
                context.SaveChanges();

                dataGrid.ItemsSource = context.buyers.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is buyers)
            {
                var selectedBuyer = dataGrid.SelectedItem as buyers;

                context.buyers.Remove(selectedBuyer);
                context.SaveChanges();

                dataGrid.ItemsSource = context.buyers.ToList();
            }
        }

        private bool TryInputNickname(out string nickname)
        {
            nickname = nicknameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(nickname))
            {
                MessageBox.Show("Введите никнейм!");
                return false;
            }

            Regex regex = new Regex(@"^[a-zA-Z0-9_]+$");
            if (!regex.IsMatch(nickname))
            {
                MessageBox.Show("Никнейм должен состоять только из английских букв, цифр и символов подчеркивания!");
                return false;
            }

            return true;
        }
    }
}

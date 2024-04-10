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
    /// Логика взаимодействия для GamesPage.xaml
    /// </summary>
    public partial class GamesPage : Page
    {
        private ClubEntities context = new ClubEntities();

        public GamesPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.games.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is games)
            {
                var _games = dataGrid.SelectedItem as games;

                titleTextBox.Text = _games.title;
                genreTextBox.Text = _games.genre;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            var newGame = new games
            {
                title = titleTextBox.Text,
                genre = genreTextBox.Text
            };

            context.games.Add(newGame);
            context.SaveChanges();

            dataGrid.ItemsSource = context.games.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is games)
            {
                var selectedGame = dataGrid.SelectedItem as games;
                if (!ValidateInput())
                    return;

                selectedGame.title = titleTextBox.Text;
                selectedGame.genre = genreTextBox.Text;

                context.SaveChanges();

                dataGrid.ItemsSource = context.games.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is games)
            {
                var selectedGame = dataGrid.SelectedItem as games;

                context.games.Remove(selectedGame);
                context.SaveChanges();

                dataGrid.ItemsSource = context.games.ToList();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text) || string.IsNullOrWhiteSpace(genreTextBox.Text))
            {
                MessageBox.Show("Заполните все поля для игры!");
                return false;
            }

            return true;
        }
    }

}

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
    /// Логика взаимодействия для AvailableGamesPage.xaml
    /// </summary>
    public partial class AvailableGamesPage : Page
    {
        public Brush PageBackgroundColor { get; set; }
        private ClubEntities context = new ClubEntities();
        public AvailableGamesPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
            PageBackgroundColor = Brushes.White;
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.availablegames.ToList();
            placeComboBox.ItemsSource = context.place.ToList();
            gamesComboBox.ItemsSource = context.games.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is availablegames)
            {
                var _availablegames = dataGrid.SelectedItem as availablegames;

                var selectedPlace = context.place.FirstOrDefault(p => p.ID_place == _availablegames.place_ID);
                placeComboBox.SelectedItem = selectedPlace;

                var selectedGames = context.games.FirstOrDefault(g => g.ID_games == _availablegames.games_ID);
                gamesComboBox.SelectedItem = selectedGames;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (placeComboBox.SelectedItem == null || gamesComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите место и игру перед созданием записи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int placeId = (placeComboBox.SelectedItem as place).ID_place;
            int gameId = (gamesComboBox.SelectedItem as games).ID_games;

            if (context.availablegames.Any(ag => ag.place_ID == placeId && ag.games_ID == gameId))
            {
                MessageBox.Show("Данная комбинация места и игры уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            availablegames _availablegames = new availablegames();

            _availablegames.place_ID = (placeComboBox.SelectedItem as place).ID_place;
            _availablegames.games_ID = (gamesComboBox.SelectedItem as games).ID_games;

            context.availablegames.Add(_availablegames);
            context.SaveChanges();
            dataGrid.ItemsSource = context.availablegames.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is availablegames)
            {
                if (placeComboBox.SelectedItem == null || gamesComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите место и игру перед редактированием записи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var _availablegames = dataGrid.SelectedItem as availablegames;
                int placeId = (placeComboBox.SelectedItem as place).ID_place;
                int gameId = (gamesComboBox.SelectedItem as games).ID_games;

                if (context.availablegames.Any(ag => ag.place_ID == placeId && ag.games_ID == gameId && ag.ID_availablegames != _availablegames.ID_availablegames))
                {
                    MessageBox.Show("Данная комбинация места и игры уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                _availablegames.place_ID = (placeComboBox.SelectedItem as place).ID_place;
                _availablegames.games_ID = (gamesComboBox.SelectedItem as games).ID_games;

                context.SaveChanges();
                dataGrid.ItemsSource = context.availablegames.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is availablegames)
            {
                var _availablegames = dataGrid.SelectedItem as availablegames;
                context.availablegames.Remove(_availablegames);
                context.SaveChanges();
                dataGrid.ItemsSource = context.availablegames.ToList();
            }
        }
        private void MainPage_MouseEnter(object sender, MouseEventArgs e)
        {
            PageBackgroundColor = Brushes.Lavender;
        }

        private void MainPage_MouseLeave(object sender, MouseEventArgs e)
        {
            PageBackgroundColor = Brushes.White;
        }

    }
}

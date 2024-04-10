using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace BD5.Pages
{
    /// <summary>
    /// Логика взаимодействия для TypeOfProblemPage.xaml
    /// </summary>
    public partial class TypeOfProblemPage : Page
    {
        private ClubEntities context = new ClubEntities();

        public TypeOfProblemPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.typeofproblem.ToList();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(categoryTextBox.Text))
            {
                if (IsValidRussianText(categoryTextBox.Text))
                {
                    var _typeofproblem = new typeofproblem();
                    _typeofproblem.category = categoryTextBox.Text;

                    context.typeofproblem.Add(_typeofproblem);
                    context.SaveChanges();

                    dataGrid.ItemsSource = context.typeofproblem.ToList();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, используйте только русские буквы для категории проблемы.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите категорию проблемы.");
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is typeofproblem)
            {
                var _typeofproblem = dataGrid.SelectedItem as typeofproblem;

                if (!string.IsNullOrWhiteSpace(categoryTextBox.Text))
                {
                    if (IsValidRussianText(categoryTextBox.Text))
                    {
                        _typeofproblem.category = categoryTextBox.Text;

                        context.SaveChanges();

                        dataGrid.ItemsSource = context.typeofproblem.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, используйте только русские буквы для категории проблемы.");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите категорию проблемы.");
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is typeofproblem)
            {
                var selectedTypeofproblem = dataGrid.SelectedItem as typeofproblem;

                context.typeofproblem.Remove(selectedTypeofproblem);
                context.SaveChanges();

                dataGrid.ItemsSource = context.typeofproblem.ToList();
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is typeofproblem)
            {
                var _typeofproblem = dataGrid.SelectedItem as typeofproblem;

                categoryTextBox.Text = _typeofproblem.category;
            }
        }

        private bool IsValidRussianText(string text)
        {
            Regex regex = new Regex(@"^[А-Яа-я\s]+$");
            return regex.IsMatch(text);
        }
    }
}

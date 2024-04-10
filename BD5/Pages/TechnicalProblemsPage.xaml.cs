using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для TechnicalProblemsPage.xaml
    /// </summary>
    public partial class TechnicalProblemsPage : Page
    {
        private ClubEntities context = new ClubEntities();
        public TechnicalProblemsPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.technicalproblems.ToList();
            typeofproblemComboBox.ItemsSource = context.typeofproblem.ToList();
            placeComboBox.ItemsSource = context.place.ToList();
            warehouseComboBox.ItemsSource = context.warehouse.ToList();
            applicationstatusComboBox.ItemsSource = context.technicalproblems
                .Select(x => x.applicationstatus).Distinct().ToList();
        }


        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is technicalproblems)
            {
                var _technicalproblems = dataGrid.SelectedItem as technicalproblems;

                var selectedtypeofproblem = context.typeofproblem.FirstOrDefault(t => t.ID_typeofproblem == _technicalproblems.typeofproblem_ID);
                typeofproblemComboBox.SelectedItem = selectedtypeofproblem;
                descriptionofproblemTextBox.Text = _technicalproblems.descriptionofproblem;
                applicationstatusComboBox.SelectedItem = _technicalproblems.applicationstatus;
                var selectedplace = context.place.FirstOrDefault(p => p.ID_place == _technicalproblems.place_ID);
                placeComboBox.SelectedItem = selectedplace;
                var selectedwarehouse = context.warehouse.FirstOrDefault(w => w.ID_warehouse == _technicalproblems.warehouse_ID);
                warehouseComboBox.SelectedItem = selectedwarehouse;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            technicalproblems _technicalproblems = new technicalproblems();

            _technicalproblems.typeofproblem_ID = (typeofproblemComboBox.SelectedItem as typeofproblem).ID_typeofproblem;
            if (!IsRussian(descriptionofproblemTextBox.Text))
            {
                MessageBox.Show("Описание проблемы должно быть на русском языке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _technicalproblems.descriptionofproblem = descriptionofproblemTextBox.Text;
            _technicalproblems.applicationstatus = applicationstatusComboBox.SelectedItem.ToString();
            _technicalproblems.place_ID = (placeComboBox.SelectedItem as place).ID_place;
            _technicalproblems.warehouse_ID = (warehouseComboBox.SelectedItem as warehouse).ID_warehouse;
            _technicalproblems.roles_ID = 2;
            if (_technicalproblems.place_ID != _technicalproblems.warehouse_ID)
            {
                MessageBox.Show("Выбранный склад не соответствует выбранному компьютеру!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.technicalproblems.Add(_technicalproblems);
            context.SaveChanges();
            dataGrid.ItemsSource = context.technicalproblems.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is technicalproblems)
            {
                if (!ValidateInputs())
                    return;

                var _technicalproblems = dataGrid.SelectedItem as technicalproblems;

                _technicalproblems.typeofproblem_ID = (typeofproblemComboBox.SelectedItem as typeofproblem).ID_typeofproblem;
                if (!IsRussian(descriptionofproblemTextBox.Text))
                {
                    MessageBox.Show("Описание проблемы должно быть на русском языке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                _technicalproblems.descriptionofproblem = descriptionofproblemTextBox.Text;
                _technicalproblems.applicationstatus = applicationstatusComboBox.SelectedItem.ToString();
                _technicalproblems.place_ID = (placeComboBox.SelectedItem as place).ID_place;
                _technicalproblems.warehouse_ID = (warehouseComboBox.SelectedItem as warehouse).ID_warehouse;
                _technicalproblems.roles_ID = 2;
                if (_technicalproblems.place_ID != _technicalproblems.warehouse_ID)
                {
                    MessageBox.Show("Выбранный склад не соответствует выбранному компьютеру!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                context.SaveChanges();
                dataGrid.ItemsSource = context.technicalproblems.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is technicalproblems)
            {
                var _technicalproblems = dataGrid.SelectedItem as technicalproblems;
                context.technicalproblems.Remove(_technicalproblems);
                context.SaveChanges();
                dataGrid.ItemsSource = context.technicalproblems.ToList();
            }
        }


        private bool IsRussian(string text)
        {
            return Regex.IsMatch(text, @"^[а-яА-Я\s]+$");
        }




        private bool ValidateInputs()
        {
            if (typeofproblemComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите тип проблемы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (placeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите место.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (warehouseComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите склад.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (applicationstatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите статус заявки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(descriptionofproblemTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите описание проблемы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}

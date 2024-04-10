using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
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
    /// Логика взаимодействия для AvailableRolesPage.xaml
    /// </summary>
    public partial class AvailableRolesPage : Page
    {
        private ClubEntities context = new ClubEntities();
        public AvailableRolesPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.availableroles.ToList();
            rolesComboBox.ItemsSource = context.roles.ToList();
            staffComboBox.ItemsSource = context.staff.ToList();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (rolesComboBox.SelectedItem == null || staffComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль и персонал перед созданием записи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int rolesId = (rolesComboBox.SelectedItem as roles).ID_roles;
            int staffId = (staffComboBox.SelectedItem as staff).ID_staff;

            if (context.availableroles.Any(ar => ar.roles_ID == rolesId && ar.staff_ID == staffId))
            {
                MessageBox.Show("Данная комбинация роли и персонала уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            availableroles _availableroles = new availableroles();

            _availableroles.roles_ID = (rolesComboBox.SelectedItem as roles).ID_roles;
            _availableroles.staff_ID = (staffComboBox.SelectedItem as staff).ID_staff;


            context.availableroles.Add(_availableroles);
            context.SaveChanges();
            dataGrid.ItemsSource = context.availableroles.ToList();

        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is availableroles)
            {
                var _availableroles = dataGrid.SelectedItem as availableroles;

                int rolesId = (rolesComboBox.SelectedItem as roles).ID_roles;
                int staffId = (staffComboBox.SelectedItem as staff).ID_staff;

                if (context.availableroles.Any(ar => ar.roles_ID == rolesId && ar.staff_ID == staffId))
                {
                    MessageBox.Show("Данная комбинация роли и персонала уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _availableroles.roles_ID = (rolesComboBox.SelectedItem as roles).ID_roles;
                _availableroles.staff_ID = (staffComboBox.SelectedItem as staff).ID_staff;

                context.SaveChanges();
                dataGrid.ItemsSource = context.availableroles.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is availableroles)
            {
                var _availableroles = dataGrid.SelectedItem as availableroles;
                context.availableroles.Remove(_availableroles);
                context.SaveChanges();
                dataGrid.ItemsSource = context.availableroles.ToList();
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is availableroles)
            {
                var _availableroles = dataGrid.SelectedItem as availableroles;

                var selectedRole = context.roles.FirstOrDefault(r => r.ID_roles == _availableroles.roles_ID);
                rolesComboBox.SelectedItem = selectedRole;

                var selectedStaff = context.staff.FirstOrDefault(s => s.ID_staff == _availableroles.staff_ID);
                staffComboBox.SelectedItem = selectedStaff;
            }
        }
    }
}

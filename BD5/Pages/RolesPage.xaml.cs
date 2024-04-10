using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BD5.Pages
{
    /// <summary>
    /// Interaction logic for RolesPage.xaml
    /// </summary>
    public partial class RolesPage : Page
    {
        private ClubEntities context = new ClubEntities();

        public RolesPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.roles.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is roles)
            {
                var _roles = dataGrid.SelectedItem as roles;
                rolesTextBox.Text = _roles.rolename;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rolesTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите наименование роли.");
                return;
            }

            var _roles = new roles();
            _roles.rolename = rolesTextBox.Text;
            context.roles.Add(_roles);
            context.SaveChanges();

            dataGrid.ItemsSource = context.roles.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is roles)
            {
                var _roles = dataGrid.SelectedItem as roles;

                if (string.IsNullOrWhiteSpace(rolesTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите наименование роли.");
                    return;
                }

                _roles.rolename = rolesTextBox.Text;
                context.SaveChanges();

                dataGrid.ItemsSource = context.roles.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is roles)
            {
                var selectedRoles = dataGrid.SelectedItem as roles;

                context.roles.Remove(selectedRoles);
                context.SaveChanges();

                dataGrid.ItemsSource = context.roles.ToList();
            }
        }
    }
}

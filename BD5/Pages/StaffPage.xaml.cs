using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BD5.Pages
{
    /// <summary>
    /// Логика взаимодействия для StaffPage.xaml
    /// </summary>
    public partial class StaffPage : Page
    {
        private ClubEntities context = new ClubEntities();

        public StaffPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.staff.ToList();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
                return;

            var _staff = new staff();
            var _extraInformation = new extrainformation();

            _staff.firstname = nameTextBox.Text;
            _staff.surname = surnameTextBox.Text;
            _staff.patronymic = patronymicTextBox.Text;
            _extraInformation.phonenumber = phoneNumberTextBox.Text;
            _extraInformation.salary = Convert.ToInt32(salaryTextBox.Text);
            _extraInformation.mail = mailTextBox.Text;
            _staff.username = loginTextBox.Text;
            _staff.passwd = //passwordTextBox.Text;
                ApplicationGlobalMethods.Instance.ComputeSHA256Hash(passwordBox.Password);


            _staff.extrainformation = _extraInformation;

            if (!IsSingleLoginAndPassword(loginTextBox.Text, passwordBox.Password))
            {
                MessageBox.Show("Такой пароль или логин уже существует.");
                return;
            }

            context.staff.Add(_staff);
            context.SaveChanges();
            MessageBox.Show("Новому сотруднику необходимо присвоить роль.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            dataGrid.ItemsSource = context.staff.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is staff)
            {
                if (!ValidateFields())
                    return;

                var _staff = dataGrid.SelectedItem as staff;

                _staff.firstname = nameTextBox.Text;
                _staff.surname = surnameTextBox.Text;
                _staff.patronymic = patronymicTextBox.Text;
                _staff.extrainformation.phonenumber = phoneNumberTextBox.Text;
                _staff.extrainformation.salary = Convert.ToInt32(salaryTextBox.Text);
                _staff.extrainformation.mail = mailTextBox.Text;
                _staff.username = loginTextBox.Text;
                _staff.passwd = //passwordTextBox.Text;
                    ApplicationGlobalMethods.Instance.ComputeSHA256Hash(passwordBox.Password);

                context.SaveChanges();

                dataGrid.ItemsSource = context.staff.ToList();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is staff)
            {
                var _staff = dataGrid.SelectedItem as staff;

                context.staff.Remove(_staff);
                context.SaveChanges();

                dataGrid.ItemsSource = context.staff.ToList();
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is staff)
            {
                var _staff = dataGrid.SelectedItem as staff;

                nameTextBox.Text = _staff.firstname;
                surnameTextBox.Text = _staff.surname;
                patronymicTextBox.Text = _staff.patronymic;
                phoneNumberTextBox.Text = _staff.extrainformation.phonenumber;
                salaryTextBox.Text = _staff.extrainformation.salary.ToString();
                mailTextBox.Text = _staff.extrainformation.mail;
                loginTextBox.Text = _staff.username;
                passwordBox.Password = _staff.passwd;
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(surnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(phoneNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(salaryTextBox.Text) ||
                string.IsNullOrWhiteSpace(mailTextBox.Text) ||
                string.IsNullOrWhiteSpace(loginTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return false;
            }

            if (!IsValidPhoneNumber(phoneNumberTextBox.Text))
            {
                MessageBox.Show("Введите корректный номер телефона в формате 89XXXXXXXXX.");
                return false;
            }

            if (!IsValidEmail(mailTextBox.Text))
            {
                MessageBox.Show("Введите корректный адрес электронной почты.");
                return false;
            }

            int salary;
            if (!int.TryParse(salaryTextBox.Text, out salary) || salary <= 0)
            {
                MessageBox.Show("Введите корректную зарплату.");
                return false;
            }

            return true;
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^89\d{9}$");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^\S+@\S+\.\S+$");
        }

        private bool IsSingleLoginAndPassword(string login, string password)
        {
            var logins = context.staff.Select(x => x.username).ToList();
            var passwords = context.staff.Select(x => x.passwd).ToList();

            foreach(var log in logins)
            {
                if (log == login)
                {
                    return false;
                }
            }
            foreach (var pass in passwords)
            {
                if (pass == password)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

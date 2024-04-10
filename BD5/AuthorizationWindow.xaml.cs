using BD5.Roles;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace BD5
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private bool firstLoad = true;
        public AuthorizationWindow()
        {
            InitializeComponent();
            if (firstLoad)
            {
                FirstLoadUpdate();
            }
        }

        private void FirstLoadUpdate()
        {
            var context = new ClubEntities();
            var staffList = context.staff.ToList();
            for (int i = 0; i < staffList.Count; i++)
            {
                if (staffList[i].passwd.Length < 64)
                {
                    staffList[i].passwd = ApplicationGlobalMethods.Instance.ComputeSHA256Hash(staffList[i].passwd);
                }
            }
            context.SaveChanges();
            
            var order_IDs = context.orders.Select(x => x.ID_orders).ToList();
            var orderFilesTxt = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\orders");
            foreach(var order_ID in order_IDs)
            {
                var fileName = Directory.GetCurrentDirectory() + "\\orders\\Заказ " + order_ID + ".txt";
                if (!File.Exists(fileName))
                {
                    ApplicationGlobalMethods.Instance
                        .CreateOrEditOrderTxtFile(context.orders.Where(x => x.ID_orders == order_ID).First());
                }
            }

            firstLoad = false;
        }

        private void authorizationButton_Click(object sender, RoutedEventArgs e)
        {
            var login = loginTextBox.Text;
            var password = passwordBox.Password;

            List<TabInfo> tabs =
                Admin.Instance.Authorization(login, password)
                .Concat(Engineer.Instance.Authorization(login, password))
                .Concat(Cashier.Instance.Authorization(login, password))
                .Distinct()
                .ToList();

            if (tabs.Count == 0)
            {
                MessageBox.Show("Пользователь не найден!");
                return;
            }

            WindowManager.Instance.OpenNewWindow(new EmployeeWindow(tabs), this);
        }

    }
}
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace BD5
{
    public class ApplicationGlobalMethods : Singleton
    {
        private ApplicationGlobalMethods() { }

        public static ApplicationGlobalMethods Instance => Instance<ApplicationGlobalMethods>();

        public string OrdersFolderPath = Directory.GetCurrentDirectory() + "\\orders";

        public string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Преобразуем строку в массив байт
                byte[] bytes = Encoding.UTF8.GetBytes(input);

                // Вычисляем хэш
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Преобразуем массив байт хэша обратно в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Преобразование в шестнадцатеричную строку
                }
                return builder.ToString();
            }
        }

        public void CreateOrEditOrderTxtFile(orders _orders)
        {
            using (StreamWriter streamWriter 
                = new StreamWriter($"{OrdersFolderPath}\\Заказ {_orders.ID_orders}.txt"))
            {
                streamWriter.WriteLine("\t\tКомпьютерный клуб");
                streamWriter.WriteLine("\t\tКассовый чек №" + _orders.ID_orders);
                streamWriter.WriteLine("");
                streamWriter.WriteLine("\tВремя начала\t\t-\t" + _orders.startime);
                streamWriter.WriteLine("\tВремя окончания\t\t-\t" + _orders.endtime);
                streamWriter.WriteLine("\tДлина сессии (мин.)\t-\t" + _orders.sessionlengths.timerate);
                streamWriter.WriteLine("\tАкция\t\t\t-\t" + _orders.promotion.nameofpromotion);
                streamWriter.WriteLine("\tМесто\t\t\t-\t" + _orders.place.nameofcomputer);
                streamWriter.WriteLine("");
                streamWriter.WriteLine("Итого к оплате (руб.) :\t" + _orders.totalamount);
                streamWriter.WriteLine("Оплачено (руб.) :\t" + _orders.totalamount);
                streamWriter.WriteLine("Сдача (руб.) :\t" + 0);
            }
        }

        public void DeleteOrderTxtFile(orders _orders)
        {
            // Проверяем, существует ли указанная директория
            if (Directory.Exists(OrdersFolderPath))
            {
                // Получаем список файлов в директории
                var files = Directory.GetFiles(OrdersFolderPath);

                // Выводим список файлов
                foreach (var file in files)
                {
                    var order_ID = file.Split('\\').Last()
                        .Substring(0, file.Split('\\').Last().Length - ".txt".Length)
                        .Split().Last();
                    if (order_ID == _orders.ID_orders.ToString())
                    {
                        if (File.Exists(file))
                        { 
                            File.Delete(file);
                            break;
                        }
                        else
                        {
                            MessageBox.Show(
                                "Чек заказа " + order_ID + " не существует!",
                                string.Empty,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "Директория для сохранения чеков не существует!",
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}

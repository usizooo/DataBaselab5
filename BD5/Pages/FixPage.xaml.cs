using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BD5.Pages
{
    /// <summary>
    /// Логика взаимодействия для WarehousePage.xaml
    /// </summary>
    public partial class FixPage : Page
    {
        private ClubEntities context = new ClubEntities();
        private int warehouseSelectedColumnIndex = -1;
        private bool pickUpPerformed = false;

        public FixPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
            fixButton.IsEnabled = false; 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            technicalProblemDataGrid.ItemsSource = context.technicalproblems.ToList();
            warehouseDataGrid.ItemsSource = context.warehouse.ToList();
        }

        private void fixButton_Click(object sender, RoutedEventArgs e)
        {
            if (!pickUpPerformed)
            {
                MessageBox.Show("Сначала возьмите необходимую комплектующую/девайс.");
                return;
            }

            if (technicalProblemDataGrid.SelectedItem != null
                && technicalProblemDataGrid.SelectedItem is technicalproblems)
            {
                var _technicalProblems = context.technicalproblems
                    .ToList()[technicalProblemDataGrid.SelectedIndex];
                if (_technicalProblems.applicationstatus == "готово")
                {
                    MessageBox.Show("Заявка уже выполнена.");
                    return;
                }
                _technicalProblems.applicationstatus = "готово";

                var _place = context.place
                    .Where(x => x.ID_place == _technicalProblems.place_ID)
                    .FirstOrDefault();

                if (_place == null)
                {
                    MessageBox.Show(
                        "Не найден компьютер соответствующий данной заявке!",
                        string.Empty,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                _place.employmentstatus = "свободен";

                context.SaveChanges();
                technicalProblemDataGrid.ItemsSource = context.technicalproblems.ToList();
            }
        }

        private void pickUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (warehouseDataGrid.SelectedItem != null
                && warehouseDataGrid.SelectedItem is warehouse
                && warehouseSelectedColumnIndex != -1)
            {
                pickUpPerformed = true;
                fixButton.IsEnabled = true; // кнопка fix доступна после выполнения операции pickup

                var _warehouse = context.warehouse.ToList()[warehouseDataGrid.SelectedIndex];
                switch (warehouseSelectedColumnIndex)
                {
                    case 2:
                        _warehouse.processor = "Нет на складе";
                        break;
                    case 3:
                        _warehouse.motherboard = "Нет на складе";
                        break;
                    case 4:
                        _warehouse.computercase = "Нет на складе";
                        break;
                    case 5:
                        _warehouse.videocard = "Нет на складе";
                        break;
                    case 6:
                        _warehouse.cpucooling = "Нет на складе";
                        break;
                    case 7:
                        _warehouse.ram = "Нет на складе";
                        break;
                    case 8:
                        _warehouse.datastorage = "Нет на складе";
                        break;
                    case 9:
                        _warehouse.powersuppply = "Нет на складе";
                        break;
                    case 10:
                        _warehouse.mouse = "Нет на складе";
                        break;
                    case 11:
                        _warehouse.headphones = "Нет на складе";
                        break;
                    case 12:
                        _warehouse.keyboard = "Нет на складе";
                        break;
                    case 13:
                        _warehouse.microphone = "Нет на складе";
                        break;
                    case 14:
                        _warehouse.screen = "Нет на складе";
                        break;
                }
                context.SaveChanges();
                warehouseDataGrid.ItemsSource = context.warehouse.ToList();
            }
            else
            {
                MessageBox.Show("Комплектующая не выбрана!");
            }
        }

        private void warehouseDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (warehouseDataGrid.SelectedItem != null && warehouseDataGrid.SelectedItem is warehouse)
            {
                var _warehouse = warehouseDataGrid.SelectedItem as warehouse;

                string selectedColumnHeader = warehouseDataGrid.CurrentColumn.Header.ToString();

                switch (selectedColumnHeader)
                {
                    case "Процессор":
                        warehouseTextBox.Text = _warehouse.processor;
                        warehouseSelectedColumnIndex = 2;
                        break;
                    case "Материнская плата":
                        warehouseTextBox.Text = _warehouse.motherboard;
                        warehouseSelectedColumnIndex = 3;
                        break;
                    case "Корпус":
                        warehouseTextBox.Text = _warehouse.computercase;
                        warehouseSelectedColumnIndex = 4;
                        break;
                    case "Видеокарта":
                        warehouseTextBox.Text = _warehouse.videocard;
                        warehouseSelectedColumnIndex = 5;
                        break;
                    case "Охлаждение процессора":
                        warehouseTextBox.Text = _warehouse.cpucooling;
                        warehouseSelectedColumnIndex = 6;
                        break;
                    case "Оперативная память":
                        warehouseTextBox.Text = _warehouse.ram;
                        warehouseSelectedColumnIndex = 7;
                        break;
                    case "Жесткий диск":
                        warehouseTextBox.Text = _warehouse.datastorage;
                        warehouseSelectedColumnIndex = 8;
                        break;
                    case "Блок питания":
                        warehouseTextBox.Text = _warehouse.powersuppply;
                        warehouseSelectedColumnIndex = 9;
                        break;
                    case "Мышка":
                        warehouseTextBox.Text = _warehouse.mouse;
                        warehouseSelectedColumnIndex = 10;
                        break;
                    case "Наушники":
                        warehouseTextBox.Text = _warehouse.headphones;
                        warehouseSelectedColumnIndex = 11;
                        break;
                    case "Клавиатура":
                        warehouseTextBox.Text = _warehouse.keyboard;
                        warehouseSelectedColumnIndex = 12;
                        break;
                    case "Микрофон":
                        warehouseTextBox.Text = _warehouse.microphone;
                        warehouseSelectedColumnIndex = 13;
                        break;
                    case "Монитор":
                        warehouseTextBox.Text = _warehouse.screen;
                        warehouseSelectedColumnIndex = 14;
                        break;
                    default:
                        warehouseTextBox.Text = "Unknown Property";
                        warehouseSelectedColumnIndex = -1;
                        break;
                }
            }
            else
            {
                warehouseSelectedColumnIndex = -1;
            }
        }
    }
}

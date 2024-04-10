using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BD5.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private ClubEntities context = new ClubEntities();

        public OrdersPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.orders.ToList();
            sessionlengthsComboBox.ItemsSource = context.sessionlengths.ToList();
            promotionComboBox.ItemsSource = context.promotion.ToList();
            placeComboBox.ItemsSource = context.place.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is orders)
            {
                var _orders = dataGrid.SelectedItem as orders;

                startimeTimePicker.SelectedTime = new DateTime().Add(_orders.startime);
                endtimeTimePicker.SelectedTime = new DateTime().Add(_orders.endtime);

                var selectedsessionlengths = context.sessionlengths.FirstOrDefault(s => s.ID_sessionlengths == _orders.sessionlengths_ID);
                var selectedpromotion = context.promotion.FirstOrDefault(p => p.ID_promotion == _orders.promotion_ID);
                var selectedPlace = context.place.FirstOrDefault(pl => pl.ID_place == _orders.place_ID);

                sessionlengthsComboBox.SelectedItem = selectedsessionlengths;
                promotionComboBox.SelectedItem = selectedpromotion;
                placeComboBox.SelectedItem = selectedPlace;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            var selectedPlace = placeComboBox.SelectedItem as place;

            if (selectedPlace.employmentstatus != "свободен")
            {
                MessageBox.Show("Выбранное место занято или находится на ремонте. Выберите другое место.");
                return;
            }

            var _orders = new orders();
            var selectedSessionLengths = sessionlengthsComboBox.SelectedItem as sessionlengths;
            var selectedPromotion = promotionComboBox.SelectedItem as promotion;

            

            _orders.startime = new TimeSpan(
                startimeTimePicker.SelectedTime.Value.Hour,
                startimeTimePicker.SelectedTime.Value.Minute,
                startimeTimePicker.SelectedTime.Value.Second);
            _orders.endtime = new TimeSpan(
                endtimeTimePicker.SelectedTime.Value.Hour,
                endtimeTimePicker.SelectedTime.Value.Minute,
                endtimeTimePicker.SelectedTime.Value.Second);
            _orders.sessionlengths = selectedSessionLengths;
            _orders.promotion = selectedPromotion;
            _orders.place = selectedPlace;
            _orders.totalamount = _orders.place.price * _orders.sessionlengths.timerate;
            
            var occupiedPlaces = context.orders.Where(o => o.ID_orders != _orders.ID_orders).Select(o => o.place);
            if (occupiedPlaces.Any(p => p.ID_place == selectedPlace.ID_place))
            {
                MessageBox.Show("Место уже занято в другом заказе. Выберите другое место.");
                return;
            }

            context.orders.Add(_orders);

            selectedPlace.employmentstatus = "занят";

            context.SaveChanges();

            dataGrid.ItemsSource = context.orders.ToList();

            // выгрузка чека в txt
            ApplicationGlobalMethods.Instance.CreateOrEditOrderTxtFile(_orders);
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is orders)
            {
                if (!ValidateInput())
                    return;

                var _orders = dataGrid.SelectedItem as orders;
                var selectedPlace = placeComboBox.SelectedItem as place;

                if (selectedPlace.employmentstatus != "свободен")
                {
                    MessageBox.Show("Выбранное место занято или находится на ремонте. Выберите другое место.");
                    return;
                }
                
                var oldPlace = _orders.place;
                oldPlace.employmentstatus = "свободен";

                _orders.startime = new TimeSpan(
                    startimeTimePicker.SelectedTime.Value.Hour,
                    startimeTimePicker.SelectedTime.Value.Minute,
                    startimeTimePicker.SelectedTime.Value.Second);
                _orders.endtime = new TimeSpan(
                    endtimeTimePicker.SelectedTime.Value.Hour,
                    endtimeTimePicker.SelectedTime.Value.Minute,
                    endtimeTimePicker.SelectedTime.Value.Second);
                _orders.sessionlengths = sessionlengthsComboBox.SelectedItem as sessionlengths;
                _orders.promotion = promotionComboBox.SelectedItem as promotion;
                _orders.place = selectedPlace;
                _orders.totalamount = _orders.place.price * _orders.sessionlengths.timerate;

                var occupiedPlaces = context.orders.Where(o => o.ID_orders != _orders.ID_orders).Select(o => o.place);
                if (occupiedPlaces.Any(p => p.ID_place == selectedPlace.ID_place))
                {
                    MessageBox.Show("Место уже занято в другом заказе. Выберите другое место.");
                    return;
                }

                selectedPlace.employmentstatus = "занят";


                context.SaveChanges();

                dataGrid.ItemsSource = context.orders.ToList();

                // изменение чека в txt
                ApplicationGlobalMethods.Instance.CreateOrEditOrderTxtFile(_orders);
            }
        }




        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is orders)
            {
                var _orders = dataGrid.SelectedItem as orders;
                var place = _orders.place;

                context.orders.Remove(_orders);

                place.employmentstatus = "свободен";

                context.SaveChanges();

                dataGrid.ItemsSource = context.orders.ToList();

                // удаление файла чека txt
                ApplicationGlobalMethods.Instance.DeleteOrderTxtFile(_orders);
            }
        }

        private bool ValidateInput()
        {
            if (startimeTimePicker.SelectedTime == null || endtimeTimePicker.SelectedTime == null)
            {
                MessageBox.Show("Не выбрано время начала или окончания сессии.");
                return false;
            }

            if (sessionlengthsComboBox.SelectedItem == null)
            {
                MessageBox.Show("Не выбрана длительность сессии.");
                return false;
            }

            if (promotionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Не выбрана акция. Если ее нет в заказе выберете поле \"Акция отсутствует\" ");
                return false;
            }

            if (placeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Не выбрано место.");
                return false;
            }

            return true;
        }
    }
}

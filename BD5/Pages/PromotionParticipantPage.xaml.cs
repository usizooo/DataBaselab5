using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для PromotionParticipantPage.xaml
    /// </summary>
    public partial class PromotionParticipantPage : Page
    {
        private ClubEntities context = new ClubEntities();
        public PromotionParticipantPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = context.promotionparticipant.ToList();
            buyersComboBox.ItemsSource = context.buyers.ToList();
            promotionComboBox.ItemsSource = context.promotion.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is promotionparticipant)
            {
                var _promotionparticipant = dataGrid.SelectedItem as promotionparticipant;

                var selectedBuyer = context.buyers.FirstOrDefault(b => b.ID_buyers == _promotionparticipant.buyers_ID);
                buyersComboBox.SelectedItem = selectedBuyer;

                var selectedPromotion = context.promotion.FirstOrDefault(p => p.ID_promotion == _promotionparticipant.promotion_ID);
                promotionComboBox.SelectedItem = selectedPromotion;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (buyersComboBox.SelectedItem == null || promotionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите участника и акцию перед созданием записи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int buyersId = (buyersComboBox.SelectedItem as buyers).ID_buyers;
            int promotionId = (promotionComboBox.SelectedItem as promotion).ID_promotion;

            if (context.promotionparticipant.Any(pp => pp.buyers_ID == buyersId && pp.promotion_ID == promotionId))
            {
                MessageBox.Show("Данная комбинация участника и акции уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            promotionparticipant _promotionparticipant = new promotionparticipant();

            _promotionparticipant.buyers_ID = (buyersComboBox.SelectedItem as buyers).ID_buyers;
            _promotionparticipant.promotion_ID = (promotionComboBox.SelectedItem as promotion).ID_promotion;

            context.promotionparticipant.Add(_promotionparticipant);
            context.SaveChanges();
            dataGrid.ItemsSource = context.promotionparticipant.ToList();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is promotionparticipant)
            {
                var _promotionparticipant = dataGrid.SelectedItem as promotionparticipant;

                int buyersId = (buyersComboBox.SelectedItem as buyers).ID_buyers;
                int promotionId = (promotionComboBox.SelectedItem as promotion).ID_promotion;

                if (context.promotionparticipant.Any(pp => pp.buyers_ID == buyersId && pp.promotion_ID == promotionId))
                {
                    MessageBox.Show("Данная комбинация участника и акции уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _promotionparticipant.buyers_ID = (buyersComboBox.SelectedItem as buyers).ID_buyers;
                _promotionparticipant.promotion_ID = (promotionComboBox.SelectedItem as promotion).ID_promotion;

                context.SaveChanges();
                dataGrid.ItemsSource = context.promotionparticipant.ToList();

            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is promotionparticipant)
            {
                var _promotionparticipant = dataGrid.SelectedItem as promotionparticipant;
                context.promotionparticipant.Remove(_promotionparticipant);
                context.SaveChanges();
                dataGrid.ItemsSource = context.promotionparticipant.ToList();
            }
        }
    }
}

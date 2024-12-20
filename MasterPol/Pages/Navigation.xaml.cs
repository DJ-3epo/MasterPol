using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MasterPol.Pages
{
    /// <summary>
    /// Логика взаимодействия для Navigation.xaml
    /// </summary>
    public partial class Navigation : Page
    {
        public Navigation()
        {
            InitializeComponent();
        }
        // Обработчик кнопки для просмотра списка партнеров
        private void ViewPartnersButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PartnerPage());

        }

        // Обработчик кнопки для добавления/редактирования данных о партнере
        private void AddEditPartnerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditPartners());
        }

        // Обработчик кнопки для просмотра истории реализации продукции
        private void ViewProductHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SalesHistoryPage());
        }

        // Обработчик кнопки для расчета необходимого материала
        private void CalculateMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialCalculationPage());
        }

        
    }
}

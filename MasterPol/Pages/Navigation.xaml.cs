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
            double productCount = 1000; // Пример количества продукции
            double materialRequired = CalculateMaterialRequired(productCount, 5); // Например, 5% брака
            MessageBox.Show($"Необходимое количество материала для производства {productCount} единиц продукции с учетом 5% брака: {materialRequired} единиц.");
        }

        // Метод для расчета необходимого материала с учетом брака
        private double CalculateMaterialRequired(double productCount, double defectPercentage)
        {
            // Например, на одну единицу продукции нужно 1.2 единицы материала
            double materialPerProduct = 1.2;

            // Учитываем брак
            double totalMaterialRequired = productCount * materialPerProduct;
            double materialWithDefects = totalMaterialRequired * (1 + defectPercentage / 100);

            return materialWithDefects;
        }
    }
}

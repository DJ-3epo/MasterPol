using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;


namespace MasterPol.Pages
{
    public partial class PartnerPage : Page
    {
        public PartnerPage()
        {
            InitializeComponent();
            LoadPartners();  // Загружаем список всех партнёров
        }

        // Метод для загрузки всех партнёров из базы данных
        private void LoadPartners()
        {
            try
            {
                // Контекст создается при каждом запросе
                using (var context = new Entities()) // Контекст создается здесь
                {
                    var partners = context.Partners
                                          .Include(p => p.Partners_type)
                                          .ToList();
                    PartnersListView.ItemsSource = partners;

                    // Если были уже какие-то элементы, очищаем их
                    PartnersListView.ItemsSource = null;

                    // Устанавливаем скидки для каждого партнёра
                    foreach (var partner in partners)

                    {
                        partner.Скидка = CalculateDiscount(partner);  // Расчитываем скидку для каждого партнёра
                    }

                    // Привязываем список партнёров к ListView
                    PartnersListView.ItemsSource = partners;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        // Метод для расчёта скидки партнёра
        private string CalculateDiscount(Partners partner)
        {
            // Используем свойство Количество_продукции для расчёта объема продукции
            double productAmount = partner.Partner_products.Sum(p => p.Количество_продукции);

            if (productAmount <= 10000)
                return "0%";
            else if (productAmount <= 50000)
                return "5%";
            else if (productAmount <= 100000)
                return "10%";
            else
                return "15%";
        }

        // Обработчик для обновления данных при возврате на страницу
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                LoadPartners();  // Загружаем данные снова
            }
        }
    }
}

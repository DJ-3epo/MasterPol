using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace MasterPol.Pages
{
    public partial class SalesHistoryPage : Page
    {
        public SalesHistoryPage()
        {
            InitializeComponent();
            LoadPartners();  // Загружаем список партнеров
            LoadSalesHistory();  // Загружаем все данные
        }

        private void LoadPartners()
        {
            using (var context = new Entities())
            {
                // Загружаем список партнеров
                var partners = context.Partners.ToList();

                // Проверяем, загружены ли данные
                if (partners.Any())
                {
                    PartnersComboBox.ItemsSource = partners;
                    PartnersComboBox.DisplayMemberPath = "Наименование_партнёра";  // Отображаем наименование партнера
                    PartnersComboBox.SelectedValuePath = "ID";  // Используем ID партнера
                }
                else
                {
                    MessageBox.Show("Нет данных о партнерах.");
                }
            }
        }

        private void LoadSalesHistory(int? partnerId = null)
        {
            using (var context = new Entities())
            {
                var query = context.Partner_products
                    .Include(pp => pp.Products)  // Включаем данные о продукции
                    .Include(pp => pp.Products.Product_type)  // Включаем данные о типе продукции
                    .Include(pp => pp.Partners)  // Включаем данные о партнерах
                    .AsQueryable();

                // Если выбран партнер, фильтруем по его ID
                if (partnerId.HasValue)
                {
                    query = query.Where(pp => pp.Partners.Id == partnerId.Value);
                }

                var salesHistory = query.ToList();
                SalesHistoryDataGrid.ItemsSource = salesHistory;
            }
        }

        // Обработчик изменения выбора в ComboBox
        private void PartnersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PartnersComboBox.SelectedItem != null)
            {
                var selectedPartner = (Partners)PartnersComboBox.SelectedItem;
                LoadSalesHistory(selectedPartner.Id);  // Используем ID выбранного партнера для фильтрации
            }
        }
    }
}

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
            LoadSalesHistory();
        }

        private void LoadSalesHistory()
        {
            using (var context = new Entities()) // Создаем новый экземпляр контекста
            {
                var salesHistory = context.Partner_products
                    .Include(pp => pp.Products)  // Включаем данные о продукции
                    .Include(pp => pp.Products.Product_type)  // Включаем данные о типе продукции
                    .Include(pp => pp.Partners)  // Включаем данные о партнерах
                    .ToList();

                SalesHistoryDataGrid.ItemsSource = salesHistory;
            }
        }
    }
}

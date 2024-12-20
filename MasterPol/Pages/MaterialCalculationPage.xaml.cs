using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MasterPol.Pages
{
    public partial class MaterialCalculationPage : Page
    {
        private readonly Entities _context;

        public MaterialCalculationPage()
        {
            InitializeComponent();
            _context = new Entities();
            LoadComboBoxData();
        }

        // Загрузка данных в ComboBox для типов продукции и материалов
        private void LoadComboBoxData()
        {
            // Загружаем типы продукции в ComboBox
            ProductTypeComboBox.ItemsSource = _context.Product_type.ToList();
            ProductTypeComboBox.DisplayMemberPath = "Тип_продукции";
            ProductTypeComboBox.SelectedValuePath = "id";

            // Загружаем типы материалов в ComboBox
            MaterialTypeComboBox.ItemsSource = _context.Material_type.ToList();
            MaterialTypeComboBox.DisplayMemberPath = "Тип_материала";
            MaterialTypeComboBox.SelectedValuePath = "id";
        }

        // Обработчик для изменения типа продукции
        private void ProductTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

       
        private void MaterialTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        // Метод для расчета необходимого материала с учетом брака
        private int CalculateMaterialRequired(int productTypeId, int materialTypeId, int productCount, double param1, double param2)
        {
            if (productCount <= 0 || param1 <= 0 || param2 <= 0)
            {
                return -1;
            }

            // Получаем коэффициент типа продукции по его идентификатору
            var productType = _context.Product_type.SingleOrDefault(pt => pt.id == productTypeId);
            if (productType == null)
            {
                return -1; // Не найден тип продукции
            }

            // Получаем процент брака материала по его идентификатору
            var materialType = _context.Material_type.SingleOrDefault(mt => mt.id == materialTypeId);
            if (materialType == null)
            {
                return -1; // Не найден тип материала
            }

            // Преобразуем процент брака из строки в вещественное число
            string defectPercentageString = materialType.Процент_брака_материала.Replace("%", "").Trim();
            if (!double.TryParse(defectPercentageString, out double defectPercentage))
            {
                return -1; // Ошибка преобразования процента брака
            }

            // Расчет необходимого материала на одну единицу продукции
            double materialPerUnit = param1 * param2 * productType.Коэффициент_типа_продукции;

            // Учет брака: увеличиваем материал с учетом процента брака
            double materialWithDefect = materialPerUnit * productCount * (1 + defectPercentage / 100);

            // Возвращаем целое число
            return (int)Math.Ceiling(materialWithDefect); // Округляем в большую сторону
        }

        // Обработчик события для кнопки "Рассчитать"
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка выбранных элементов ComboBox
                if (ProductTypeComboBox.SelectedItem == null || MaterialTypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите тип продукции и тип материала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Получаем выбранные данные из ComboBox
                int productTypeId = (int)ProductTypeComboBox.SelectedValue;
                int materialTypeId = (int)MaterialTypeComboBox.SelectedValue;

                // Получаем данные из текстовых полей
                if (!int.TryParse(ProductCountTextBox.Text, out int productCount) ||
                    !double.TryParse(Param1TextBox.Text, out double param1) ||
                    !double.TryParse(Param2TextBox.Text, out double param2))
                {
                    MessageBox.Show("Пожалуйста, введите корректные значения для всех параметров.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Выполняем расчет
                int result = CalculateMaterialRequired(productTypeId, materialTypeId, productCount, param1, param2);

                // Выводим результат
                if (result == -1)
                {
                    MessageBox.Show("Произошла ошибка при расчете.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ResultTextBlock.Text = $"Необходимое количество материала: {result} единиц.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

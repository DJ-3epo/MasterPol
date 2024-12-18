using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace MasterPol.Pages
{
    public partial class EditPartners : Page
    {
        public EditPartners()
        {
            InitializeComponent();
            LoadPartners();  // Загружаем список всех партнёров
        }

        // Метод для загрузки всех партнёров из базы данных
        private void LoadPartners()
        {
            try
            {
                using (var context = new Entities()) // Контекст создается здесь
                {
                    // Получаем всех партнёров с типами партнёров (используем Include для загрузки связанных данных)
                    var partners = context.Partners
                                          .Include(p => p.Partners_type)  // Включаем навигационное свойство "Partners_type"
                                          .ToList();

                    // Если были уже какие-то элементы, очищаем их
                    DataGridPartner.ItemsSource = null;

                    // Привязываем список партнёров к DataGrid
                    DataGridPartner.ItemsSource = partners;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }



        // Метод для добавления нового партнёра
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Открываем страницу добавления партнёра
            NavigationService.Navigate(new Pages.AddPartnerPage(null)); // Страница для добавления партнера
        }

        // Метод для удаления выбранных партнёров
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var partnersForRemoving = DataGridPartner.SelectedItems.Cast<Partners>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {partnersForRemoving.Count} элементов?", "Внимание",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new Entities())
                    {
                        context.Partners.RemoveRange(partnersForRemoving);
                        context.SaveChanges();
                        MessageBox.Show("Данные успешно удалены!");
                        LoadPartners();  // Загружаем данные снова
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Метод для редактирования выбранного партнёра
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранного партнера
            var selectedPartner = (sender as Button).DataContext as Partners;

            // Открываем страницу редактирования партнёра
            NavigationService.Navigate(new Pages.AddPartnerPage(selectedPartner)); // Страница для редактирования партнера
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

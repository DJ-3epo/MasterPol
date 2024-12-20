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
                    // Получаем всех партнёров с типами партнёров
                    var partners = context.Partners
                                          .Include(p => p.Partners_type)  // Включаем тип партнёра
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

       

        // Метод для редактирования выбранного партнёра
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранного партнера
            var selectedPartner = (sender as Button).DataContext as Partners;

            // Открываем страницу редактирования партнёра
            NavigationService.Navigate(new Pages.AddPartnerPage(selectedPartner)); // Страница для редактирования партнера
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранные партнёрские записи для удаления
            var partnersForRemoving = DataGridPartner.SelectedItems.Cast<Partners>().ToList();

            // Проверка, что были выбраны партнёры для удаления
            if (partnersForRemoving.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите хотя бы одного партнёра для удаления.");
                return;
            }

            // Подтверждение действия удаления
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {partnersForRemoving.Count} элементов?", "Внимание",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new Entities()) // Контекст создается здесь
                    {
                        // Загружаем партнёров из базы данных и отслеживаем их контекстом
                        foreach (var partner in partnersForRemoving)
                        {
                            // Попытка получить партнёра из базы данных для корректного отслеживания контекстом
                            var partnerToDelete = context.Partners
                                                         .FirstOrDefault(p => p.Id == partner.Id);

                            if (partnerToDelete != null)
                            {
                                // Удаляем партнёра, который отслеживается контекстом
                                context.Partners.Remove(partnerToDelete);
                            }
                        }

                        // Сохраняем изменения в базе данных
                        context.SaveChanges();

                        // Загружаем обновленный список партнёров
                        LoadPartners();

                        // Сообщаем пользователю об успешном удалении
                        MessageBox.Show("Данные успешно удалены!");
                    }
                }
                catch (Exception ex)
                {
                    // Обрабатываем ошибки
                    MessageBox.Show($"Ошибка при удалении данных: {ex.Message}");
                }
            }
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

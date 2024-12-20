using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity.Validation;
using System.Linq;

namespace MasterPol.Pages
{
    public partial class AddPartnerPage : Page
    {
        private Partners _currentPartner = new Partners();

        public AddPartnerPage(Partners selectedPartner)
        {
            InitializeComponent();

            // Загружаем типы партнёров из базы данных и привязываем их к ComboBox
            using (var context = new Entities())
            {
                var partnerTypes = context.Partners_type.ToList();
                PartnerTypeComboBox.ItemsSource = partnerTypes;
            }

            if (selectedPartner != null)
                _currentPartner = selectedPartner;

            DataContext = _currentPartner; // Устанавливаем DataContext для привязки данных

            
        }

        

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверки для новых полей
            if (string.IsNullOrWhiteSpace(_currentPartner.Наименование_партнёра))
                errors.AppendLine("Укажите наименование партнёра!");
            if (string.IsNullOrWhiteSpace(_currentPartner.Директор))
                errors.AppendLine("Укажите директора!");
            if (string.IsNullOrWhiteSpace(_currentPartner.Электронная_почта_партнёра))
                errors.AppendLine("Укажите электронную почту!");
            if (string.IsNullOrWhiteSpace(_currentPartner.Телефон_партнёра))
                errors.AppendLine("Укажите телефон!");
            if (string.IsNullOrWhiteSpace(_currentPartner.Юридический_адрес_партнера)) // Новая проверка для юридического адреса
                errors.AppendLine("Укажите юридический адрес!");
            if (string.IsNullOrWhiteSpace(_currentPartner.ИНН)) // Новая проверка для ИНН
                errors.AppendLine("Укажите ИНН!");
            if (_currentPartner.Тип_партнёра == 0 || PartnerTypeComboBox.SelectedItem == null)
                errors.AppendLine("Выберите тип партнёра!");
            if (_currentPartner.Ренйтинг < 1 || _currentPartner.Ренйтинг > 10) // Проверка для рейтинга
                errors.AppendLine("Рейтинг должен быть числом от 1 до 10!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Создаем контекст базы данных
            using (var context = new Entities())
            {
                try
                {
                    // Пытаемся добавить или обновить партнёра
                    if (_currentPartner.Id == 0)
                    {
                        // Если это новый партнёр, добавляем его
                        context.Partners.Add(_currentPartner);
                    }
                    else
                    {
                        // Если партнёр уже существует, обновляем его
                        var partnerToUpdate = context.Partners.Find(_currentPartner.Id);
                        if (partnerToUpdate != null)
                        {
                            partnerToUpdate.Наименование_партнёра = _currentPartner.Наименование_партнёра;
                            partnerToUpdate.Директор = _currentPartner.Директор;
                            partnerToUpdate.Электронная_почта_партнёра = _currentPartner.Электронная_почта_партнёра;
                            partnerToUpdate.Телефон_партнёра = _currentPartner.Телефон_партнёра;
                            partnerToUpdate.Тип_партнёра = _currentPartner.Тип_партнёра;
                            partnerToUpdate.Юридический_адрес_партнера = _currentPartner.Юридический_адрес_партнера;
                            partnerToUpdate.ИНН = _currentPartner.ИНН;
                            partnerToUpdate.Ренйтинг = _currentPartner.Ренйтинг;
                        }
                    }

                    context.SaveChanges(); // Сохраняем изменения в базе данных
                    MessageBox.Show("Данные успешно сохранены!");
                }
                catch (DbEntityValidationException ex)
                {
                    // Обрабатываем ошибку валидации
                    StringBuilder sb = new StringBuilder();
                    foreach (var entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationError.ValidationErrors)
                        {
                            sb.AppendLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                        }
                    }
                    MessageBox.Show("Ошибки валидации:\n" + sb.ToString());
                }
                catch (Exception ex)
                {
                    // Обработка других ошибок
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            // Логика отмены (например, возвращаем на предыдущую страницу)
            NavigationService.GoBack();
        }
    }
}

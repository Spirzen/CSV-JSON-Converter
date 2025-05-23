using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using CSV_JSON_Converter.Services;

namespace CSV_JSON_Converter
{
    /// <summary>
    /// Главное окно приложения для конвертации CSV в JSON.
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _csvFilePath;

        /// <summary>
        /// Инициализирует новый экземпляр класса MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Загрузить файл".
        /// Открывает диалог выбора CSV-файла и отображает его содержимое.
        /// </summary>
        private void LoadCsv_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Выберите CSV файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _csvFilePath = openFileDialog.FileName;
                    var rows = CsvService.ReadCsv(_csvFilePath);

                    var previewText = string.Join("\n", rows.Select(row =>
                        string.Join(", ", row.Select(kv => $"{kv.Key}: {kv.Value}"))));

                    CsvPreview.Text = previewText;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Преобразовать в JSON".
        /// Преобразует данные из CSV в JSON и сохраняет их в файл.
        /// </summary>
        private void ConvertToJson_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_csvFilePath))
            {
                MessageBox.Show("Сначала загрузите CSV-файл.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    Title = "Сохранить JSON файл",
                    FileName = "output.json"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string jsonFilePath = saveFileDialog.FileName;
                    var rows = CsvService.ReadCsv(_csvFilePath);
                    string json = JsonService.ConvertToColumnarJson(rows);

                    File.WriteAllText(jsonFilePath, json);
                    JsonPreview.Text = json;

                    MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при преобразовании: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
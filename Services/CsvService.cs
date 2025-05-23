using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace CSV_JSON_Converter.Services
{
    /// <summary>
    /// Предоставляет методы для работы с CSV-файлами.
    /// </summary>
    public static class CsvService
    {
        /// <summary>
        /// Читает данные из CSV-файла и возвращает их в виде списка словарей, где ключи — это заголовки столбцов.
        /// </summary>
        /// <param name="filePath">Путь к CSV-файлу.</param>
        /// <returns>Список словарей, представляющих строки CSV-файла.</returns>
        public static List<Dictionary<string, string>> ReadCsv(string filePath)
        {
            var rows = new List<Dictionary<string, string>>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var row = new Dictionary<string, string>();
                foreach (var header in csv.HeaderRecord)
                {
                    row[header] = csv.GetField(header);
                }
                rows.Add(row);
            }

            return rows;
        }
    }
}
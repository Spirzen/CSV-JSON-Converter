using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSV_JSON_Converter.Services
{
    /// <summary>
    /// Предоставляет методы для преобразования данных в JSON-формат.
    /// </summary>
    public static class JsonService
    {
        /// <summary>
        /// Преобразует список словарей (строки данных) в колоночный JSON-формат.
        /// </summary>
        /// <param name="rows">Список словарей, представляющих строки данных.</param>
        /// <returns>Строка JSON, содержащая данные в колоночном формате.</returns>
        public static string ConvertToColumnarJson(List<Dictionary<string, string>> rows)
        {
            var columnarData = new Dictionary<string, List<string>>();

            if (rows.Count == 0)
            {
                return JsonConvert.SerializeObject(columnarData, Formatting.Indented);
            }

            foreach (var key in rows[0].Keys)
            {
                columnarData[key] = new List<string>();
            }

            foreach (var row in rows)
            {
                foreach (var key in row.Keys)
                {
                    columnarData[key].Add(row[key]);
                }
            }

            return JsonConvert.SerializeObject(columnarData, Formatting.Indented);
        }
    }
}
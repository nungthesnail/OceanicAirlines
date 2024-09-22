using System.Text.Json.Serialization;

namespace AuthenticationService.Models
{
    /// <summary>
    /// Модель для сериализации и десериализации булевого результата операции
    /// </summary>
    public class BoolResponseModel
    {
        /// <summary>
        /// Результат операции
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Конструктор для JSON
        /// </summary>
        /// <param name="result">Значение результата</param>
        [JsonConstructor]
        public BoolResponseModel(bool result = false)
        {
            Result = result;
        }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public BoolResponseModel()
        { }
    }
}

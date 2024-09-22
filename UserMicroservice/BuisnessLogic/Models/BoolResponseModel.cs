using System.Text.Json.Serialization;

namespace BuisnessLogic.Models
{
    /// <summary>
    /// Класс для сериализации и десериализации булевого ответа
    /// </summary>
    public class BoolResponseModel
    {
        /// <summary>
        /// Булево значение результата операции
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Конструктор JSON
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

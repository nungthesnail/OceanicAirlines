using EntityFrameworkLogic.Entities;

namespace FlightService.Models
{
    /// <summary>
    /// Модель, представляющая сущность аэропорта
    /// </summary>
    public class AirportModel
    {
        /// <summary>
        /// Идентификатор сущности аэропорта
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код аэропорта IATA
        /// </summary>
        public string CodeIata { get; set; } = null!;

        /// <summary>
        /// Название аэропорта
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Метод построения модели аэропорта из сущности аэропорта. В будущем будет вынесен в отдельный строитель
        /// для поддержания разделения ответственности классов
        /// </summary>
        /// <param name="airport">Сущность аэропорта</param>
        /// <returns>Построенная модель аэропорта</returns>
        public static AirportModel BuildFrom(Airport airport)
        {
            return new()
            {
                Id = airport.Id,
                CodeIata = airport.CodeIata,
                Name = airport.Name
            };
        }
    }
}

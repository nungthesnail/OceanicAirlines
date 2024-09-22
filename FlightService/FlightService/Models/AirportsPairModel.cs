using EntityFrameworkLogic.Entities;

namespace FlightService.Models
{
    /// <summary>
    /// Модель, представляющая сущность пары аэропортов
    /// </summary>
    public class AirportsPairModel
    {
        /// <summary>
        /// Идентификатор сущности пары аэропортов
        /// </summary>
        public int Id { get; set; }

		/// <summary>
		/// Внешний ключ, ссылающийся на идентификатор первого аэропорта в паре
		/// </summary>
		public int FirstAirportId { get; set; }

		/// <summary>
		/// Навигационное свойство, ссылающееся на сущность первого аэропорта в паре
		/// </summary>
		public AirportModel? FirstAirport { get; set; }

		/// <summary>
		/// Внешний ключ, ссылающийся на идентификатор второго аэропорта в паре
		/// </summary>
		public int SecondAirportId { get; set; }

		/// <summary>
		/// Навигационное свойство, ссылающееся на сущность второго аэропорта в паре
		/// </summary>
		public AirportModel? SecondAirport { get; set; }

		/// <summary>
		/// Расстояние между аэропортами, указанное в километровом масштабе
		/// </summary>
		public float DistanceInKm { get; set; }

		/// <summary>
		/// Метод построения модели пары аэропортов из сущности пары аэропортов. В будущем будет вынесен в отдельный строитель
		/// для поддержания разделения ответственности классов
		/// </summary>
		/// <param name="pair">Сущность пары аэропортов</param>
		/// <returns>Построенная модель пары аэропортов</returns>
		public static AirportsPairModel BuildFrom(AirportsPair pair)
        {
            return new()
            {
                Id = pair.Id,
                FirstAirportId = pair.FirstAirportId,
                SecondAirportId = pair.SecondAirportId,
                DistanceInKm = pair.DistanceInKm,

                FirstAirport = pair.FirstAirport != null ? AirportModel.BuildFrom(pair.FirstAirport) : null,
                SecondAirport = pair.SecondAirport != null ? AirportModel.BuildFrom(pair.SecondAirport) : null
            };
        }
    }
}

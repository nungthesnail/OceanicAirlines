using EntityFrameworkLogic.Entities;

namespace FlightService.Models
{
    /// <summary>
    /// Модель, представляющая сущность базового рейса
    /// </summary>
    public class BaseFlightModel
    {
		/// <summary>
		/// Идентификатор сущности базового рейса
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Внешний ключ, ссылающийся на пару аэропортов, между которыми производятся запланированные рейсы,
		/// имеющие данный базовый рейс, в направлении от первого аэропорта в паре ко второму
		/// </summary>
		public int AirportsPairId { get; set; }

		/// <summary>
		/// Навигационное свойство, ссылающееся на пару аэропортов, между которыми производятся запланированные рейсы,
		/// имеющие данный базовый рейс, в направлении от первого аэропорта в паре ко второму
		/// </summary>
		public AirportsPairModel? AirportsPair { get; set; }

		/// <summary>
		/// Тип воздушного судна, обслуживающего запланированные рейсы, имеющие данный базовый рейс
		/// </summary>
		public string? AircraftType { get; set; }

		/// <summary>
		/// Метод построения модели базового рейса из сущности базового рейса. В будущем будет вынесен в отдельный строитель
		/// для поддержания разделения ответственности классов
		/// </summary>
		/// <param name="baseFlight">Сущность базового рейса</param>
		/// <returns>Построенная модель базового рейса</returns>
		public static BaseFlightModel BuildFrom(BaseFlight baseFlight)
        {
            return new()
            {
                Id = baseFlight.Id,
                AirportsPairId = baseFlight.AirportsPairId,
                AircraftType = baseFlight.AircraftType,

                AirportsPair = baseFlight.AirportsPair != null ? AirportsPairModel.BuildFrom(baseFlight.AirportsPair) : null
            };
        }
    }
}

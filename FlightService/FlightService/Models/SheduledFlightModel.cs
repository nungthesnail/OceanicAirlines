using EntityFrameworkLogic.Entities;

namespace FlightService.Models
{
    /// <summary>
    /// Модель, представляющая сущность запланированного рейса в базе данных
    /// </summary>
    public class SheduledFlightModel
    {
		/// <summary>
		/// Идентификатор сущности запланированного рейса
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Внешний ключ, ссылающийся на базовый рейс данного запланированного рейса
		/// </summary>
		public int BaseFlightId { get; set; }

		/// <summary>
		/// Навигационное свойство, ссылающееся на базовый рейс данного запланированного рейса
		/// </summary>
		public BaseFlightModel? BaseFlight { get; set; }

		/// <summary>
		/// Статус выполнения запланированного рейса. В данный момент не имеет применения
		/// </summary>
		public int Status { get; set; } = -1;

		/// <summary>
		/// Запланированное время вылета данного рейса
		/// </summary>
		public DateTime SheduledDeparture { get; set; }

		/// <summary>
		/// Запланированное время приземления данного рейса
		/// </summary>
		public DateTime SheduledArrival { get; set; }

		/// <summary>
		/// Число, представляющее общее количество мест, доступных для бронивания на данном запланированном рейсе.
		/// Никак не отражает количество бронирований или реальных забронированных мест на данном рейсе
		/// </summary>
		public int SeatsCount { get; set; }

		/// <summary>
		/// Метод построения модели запланированного рейса из сущности запланированного рейса. В будущем будет вынесен в отдельный строитель
		/// для поддержания разделения ответственности классов
		/// </summary>
		/// <param name="sheduledFlight">Сущность запланированного рейса</param>
		/// <returns>Построенная модель запланированного рейса</returns>
		public static SheduledFlightModel BuildFrom(SheduledFlight sheduledFlight)
        {
            return new()
            {
                Id = sheduledFlight.Id,
                BaseFlightId = sheduledFlight.BaseFlightId,
                Status = sheduledFlight.Status,
                SheduledDeparture = sheduledFlight.SheduledDeparture,
                SheduledArrival = sheduledFlight.SheduledArrival,
                SeatsCount = sheduledFlight.SeatsCount,

                BaseFlight = sheduledFlight.BaseFlight != null ? BaseFlightModel.BuildFrom(sheduledFlight.BaseFlight) : null
            };
        }
    }
}

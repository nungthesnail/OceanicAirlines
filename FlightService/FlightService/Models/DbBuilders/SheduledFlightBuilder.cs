using EntityFrameworkLogic.Entities;

namespace FlightService.Models.DbBuilders
{
	/// <summary>
	/// Строитель сущности запланированного рейса из модели запланированного рейса
	/// </summary>
	public static class SheduledFlightBuilder
    {
		/// <summary>
		/// Метод построения сущности запланированного рейса из модели запланированного рейса
		/// </summary>
		/// <param name="model">Модель запланированного рейса</param>
		/// <returns>Построенная сущность</returns>
		public static SheduledFlight BuildFrom(SheduledFlightModel model)
        {
            return new SheduledFlight
            {
                Id = model.Id,
                BaseFlightId = model.BaseFlightId,
                Status = model.Status,
                SheduledDeparture = model.SheduledDeparture,
                SheduledArrival = model.SheduledArrival,
                SeatsCount = model.SeatsCount
            };
        }
    }
}

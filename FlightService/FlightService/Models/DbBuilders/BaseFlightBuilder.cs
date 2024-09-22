using EntityFrameworkLogic.Entities;

namespace FlightService.Models.DbBuilders
{
	/// <summary>
	/// Строитель сущности базового рейса из модели базового рейса
	/// </summary>
	public static class BaseFlightBuilder
    {
		/// <summary>
		/// Метод построения сущности базового рейса из модели базового рейса
		/// </summary>
		/// <param name="model">Модель базового рейса</param>
		/// <returns>Построенная сущность</returns>
		public static BaseFlight BuildFrom(BaseFlightModel model)
        {
            return new BaseFlight
            {
                Id = model.Id,
                AirportsPairId = model.AirportsPairId,
                AircraftType = model.AircraftType
            };
        }
    }
}

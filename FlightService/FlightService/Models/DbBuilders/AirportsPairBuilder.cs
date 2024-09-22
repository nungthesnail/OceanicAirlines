using EntityFrameworkLogic.Entities;

namespace FlightService.Models.DbBuilders
{
	/// <summary>
	/// Строитель сущности пары аэропортов из модели пары аэропортов
	/// </summary>
	public static class AirportsPairBuilder
    {
		/// <summary>
		/// Метод построения сущности пары аэропортов из модели пары аэропортов
		/// </summary>
		/// <param name="model">Модель пары аэропортов</param>
		/// <returns>Построенная сущность</returns>
		public static AirportsPair BuildFrom(AirportsPairModel model)
        {
            return new AirportsPair
            {
                Id = model.Id,
                FirstAirportId = model.FirstAirportId,
                SecondAirportId = model.SecondAirportId,
                DistanceInKm = model.DistanceInKm
            };
        }
    }
}

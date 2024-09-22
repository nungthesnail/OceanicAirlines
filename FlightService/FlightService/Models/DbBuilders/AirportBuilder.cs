using EntityFrameworkLogic.Entities;

namespace FlightService.Models.DbBuilders
{
    /// <summary>
    /// Строитель сущности аэропорта из модели аэропорта
    /// </summary>
    public static class AirportBuilder
    {
        /// <summary>
        /// Метод построения сущности аэропорта из модели аэропорта
        /// </summary>
        /// <param name="airportModel">Модель аэропорта</param>
        /// <returns>Построенная сущность</returns>
        public static Airport BuildFrom(AirportModel airportModel)
        {
            return new Airport()
            {
                Id = airportModel.Id,
                CodeIata = airportModel.CodeIata,
                Name = airportModel.Name,
            };
        }
    }
}

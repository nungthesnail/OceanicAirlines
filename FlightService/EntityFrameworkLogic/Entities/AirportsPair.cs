namespace EntityFrameworkLogic.Entities
{
    /// <summary>
    /// Сущность пары аэропортов в базе данных
    /// </summary>
    public class AirportsPair
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Навигационное свойство, ссылающееся на сущность первого аэропорта в паре
        /// </summary>
        public Airport FirstAirport { get; set; } = null!;

        /// <summary>
        /// Внешний ключ, ссылающийся на идентификатор первого аэропорта в паре
        /// </summary>
        public int FirstAirportId { get; set; }

        /// <summary>
        /// Навигационное свойство, ссылающееся на сущность второго аэропорта в паре
        /// </summary>
        public Airport SecondAirport { get; set; } = null!;

        /// <summary>
        /// Внешний ключ, ссылающийся на идентификатор второго аэропорта в паре
        /// </summary>
        public int SecondAirportId { get; set; }

        /// <summary>
        /// Расстояние между аэропортами, указанное в километровом масштабе
        /// </summary>
        public float DistanceInKm { get; set; }

        /// <summary>
        /// Навигационное свойство, указывающее базовые рейсы, для пары аэропортов которых указана данная пара аэропортов
        /// </summary>
        public List<BaseFlight> BaseFlights { get; set; } = new();
    }
}

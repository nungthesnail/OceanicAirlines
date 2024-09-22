namespace EntityFrameworkLogic.Entities
{
    /// <summary>
    /// Сущность базового рейса в базе данных
    /// </summary>
    public class BaseFlight
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Навигационное свойство, ссылающееся на пару аэропортов, между которыми производятся запланированные рейсы,
        /// имеющие данный базовый рейс, в направлении от первого аэропорта в паре ко второму
        /// </summary>
        public AirportsPair AirportsPair { get; set; } = null!;

		/// <summary>
		/// Внешний ключ, ссылающийся на пару аэропортов, между которыми производятся запланированные рейсы,
		/// имеющие данный базовый рейс, в направлении от первого аэропорта в паре ко второму
		/// </summary>
		public int AirportsPairId { get; set; }

        /// <summary>
        /// Тип воздушного судна, обслуживающего запланированные рейсы, имеющие данный базовый рейс
        /// </summary>
        public string? AircraftType { get; set; }

        /// <summary>
        /// Навигационное свойство, указывающее запланированные рейсы, имеющие данный базовый рейс
        /// </summary>
        public List<SheduledFlight> SheduledFlights { get; set; } = new();
    }
}

namespace EntityFrameworkLogic.Entities
{
    /// <summary>
    /// Сущность аэропорта в базе данных
    /// </summary>
    public class Airport
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код аэропорта IATA (Международная Ассоциация Воздушного Транспорта)
        /// </summary>
        public string CodeIata { get; set; } = null!;

        /// <summary>
        /// Название аэропорта
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Навигационное свойство, указывающее на сущности пар аэропортов, в которых данный аэропорт представлен первым в паре
        /// </summary>
        public List<AirportsPair> AirportsPairsFirst { get; set; } = new();

		/// <summary>
		/// Навигационное свойство, указывающее на сущности пар аэропортов, в которых данный аэропорт представлен вторым в паре
		/// </summary>
		public List<AirportsPair> AirportsPairsSecond { get; set; } = new();
    }
}

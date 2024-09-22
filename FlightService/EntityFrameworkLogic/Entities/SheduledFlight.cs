namespace EntityFrameworkLogic.Entities
{
    /// <summary>
    /// Сущность запланированного рейса в базе данных
    /// </summary>
    public class SheduledFlight
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Навигационное свойство, ссылающееся на базовый рейс данного запланированного рейса
        /// </summary>
        public BaseFlight BaseFlight { get; set; } = null!;

        /// <summary>
        /// Внешний ключ, ссылающийся на базовый рейс данного запланированного рейса
        /// </summary>
        public int BaseFlightId { get; set; }

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
    }
}

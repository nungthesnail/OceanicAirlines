namespace BookingService.Services.RabbitMq
{
    /// <summary>
    /// Интерфейс сервиса, обеспечивающего взаимодействие с брокером сообщений
    /// </summary>
    public interface IRabbitMqService
    {
        /// <summary>
        /// Сериализует и отправляет сообщение в очередь брокера сообщений
        /// </summary>
        /// <param name="message">Объект сообщения</param>
        public void Send(object? message);

		/// <summary>
		/// Отправляет сообщение в очередь брокера сообщений
		/// </summary>
		/// <param name="message">Сообщение</param>
		public void Send(string message);
    }
}
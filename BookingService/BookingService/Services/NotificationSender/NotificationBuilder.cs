using System.Text.Json;

namespace BookingService.Services.NotificationSender
{
	/// <summary>
	/// Строитель уведомления
	/// </summary>
	public class NotificationBuilder
	{
		private Notification.ReceiversTypes _receiverType = Notification.ReceiversTypes.RECEIVER_EMAIL;

		private string? _receiverData = null;

		private string? _message = null;

		/// <summary>
		/// Устанавливает тип получения уведомления
		/// </summary>
		/// <param name="receiverType">Тип получения уведомления</param>
		public void SetReceiverType(Notification.ReceiversTypes receiverType)
		{
			_receiverType = receiverType;
		}

		/// <summary>
		/// Устанавливает данные, необходимые для адресации уведомления
		/// </summary>
		/// <param name="receiverData">Данные, необходимые для адресации уведомления</param>
		public void SetReceiverData(string receiverData)
		{
			_receiverData = receiverData;
		}

		/// <summary>
		/// Устанавливает сообщение уведомления
		/// </summary>
		/// <param name="message">Сообщение уведомления</param>
		public void SetMessage(string message)
		{
			_message = message;
		}

		/// <summary>
		/// Сериализует и устанавливает сообщение уведомления
		/// </summary>
		/// <param name="message">Объект сообщения уведомления</param>
		public void SetMessage(object? message)
		{
			_message = JsonSerializer.Serialize(message);
		}

		/// <summary>
		/// Строит уведомление
		/// </summary>
		/// <returns>Построенное уведомление</returns>
		/// <exception cref="InvalidOperationException"></exception>
		public Notification Build()
		{
			ThrowIfBuildingIncompleted();

			var result = new Notification()
			{
				ReceiverType = _receiverType,
				ReceiverData = _receiverData!,
				Message = _message!
			};

			return result;
		}

		private void ThrowIfBuildingIncompleted()
		{
			var completed = _receiverData != null && _message != null;

			if (!completed)
			{
				throw new InvalidOperationException("Notification building is incompleted");
			}
		}
	}
}

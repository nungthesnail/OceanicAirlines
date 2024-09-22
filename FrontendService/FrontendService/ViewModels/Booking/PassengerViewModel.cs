using InterserviceCommunication.Models.BookingService;


namespace FrontendService.ViewModels.Booking
{
	/// <summary>
	/// Модель вида, описывающая данные пассажира в бронировании
	/// </summary>
	public class PassengerViewModel
	{
		/// <summary>
		/// Имя пассажира
		/// </summary>
		public string FirstName { get; set; } = null!;

		/// <summary>
		/// Фамилия пассажира
		/// </summary>
		public string Surname { get; set; } = null!;

		/// <summary>
		/// Отчество пассажира при наличии
		/// </summary>
		public string? MiddleName { get; set; }

		/// <summary>
		/// Дата рождения пассажира
		/// </summary>
		public DateOnly? BirthDate { get; set; }

		/// <summary>
		/// Максимальный вес багажа пассажира в килограммах
		/// </summary>
		public float MaxBaggageWeight { get; set; } = 0.0f;

		/// <summary>
		/// Максимальный вес ручной клади пассажира в килограммах
		/// </summary>
		public float MaxCarryOnWeight { get; set; } = 0.0f;

		/// <summary>
		/// Метод, строящий модель вида пассажира из модели связки пассажир-к-бронированию для межсервисного взаимодействия.
		/// В будущем функционал будет вынесен в отдельный строитель для разделения ответственности классов.
		/// </summary>
		/// <param name="model">Связка пассажир-к-бронированию</param>
		/// <returns>Построенная модель вида пассажира</returns>
		public static PassengerViewModel BuildFrom(BookingServicePassengerToBookingModel model)
		{
			return new PassengerViewModel
			{
				MaxBaggageWeight = model.MaxBaggageWeight,
				MaxCarryOnWeight = model.MaxCarryOnWeight,

				FirstName = model.Passenger.FirstName!,
				Surname = model.Passenger.Surname!,
				MiddleName = model.Passenger.MiddleName,
				BirthDate = model.Passenger.BirthDate
			};
		}
	}
}

namespace FrontendService.Models.Booking
{
	/// <summary>
	/// Модель пассажира в бронировании
	/// </summary>
	public class PassengerModel
	{
		/// <summary>
		/// Имя пассажира
		/// </summary>
		public string? FirstName { get; set; }

		/// <summary>
		/// Фамилия пассажира
		/// </summary>
		public string? Surname { get; set; }

		/// <summary>
		/// Отчество пассажира при наличии
		/// </summary>
		public string? MiddleName { get; set; }

		/// <summary>
		/// Номер документа, удостоверяющий личность пассажира
		/// </summary>
		public string? DocumentNumber { get; set; }

		/// <summary>
		/// Гражданство пассажира
		/// </summary>
		public string? DocumentIssuerCountry { get; set; }

		/// <summary>
		/// Дата рождения пассажира
		/// </summary>
		public DateOnly? BirthDate { get; set; }

		/// <summary>
		/// Пол пассажира. 0 - мужской пол, 1 - женский пол
		/// </summary>
		public int Gender { get; set; }

		/// <summary>
		/// Номер телефона пассажира
		/// </summary>
		public string? PhoneNumber { get; set; }

		/// <summary>
		/// Необязательный адрес электронной почты пассажира
		/// </summary>
		public string? Email { get; set; }

		/// <summary>
		/// Максимальный вес ручной клади пассажира
		/// </summary>
		public float MaxCarryOnWeight { get; set; } = 10;

		/// <summary>
		/// Максимальный вес багажа пассажира
		/// </summary>
		public float MaxBaggageWeight { get; set; } = 10;
	}
}

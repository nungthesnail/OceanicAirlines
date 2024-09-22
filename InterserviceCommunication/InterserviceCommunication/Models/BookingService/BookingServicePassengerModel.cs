namespace InterserviceCommunication.Models.BookingService
{
	/// <summary>
	/// Модель пассажира
	/// </summary>
	public class BookingServicePassengerModel
	{
		/// <summary>
		/// Идентификатор пассажира
		/// </summary>
		public int Id { get; set; }

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
		/// Номер документа, удостоверяющего личность пассажира
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
		/// Пол пассажира
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
	}
}

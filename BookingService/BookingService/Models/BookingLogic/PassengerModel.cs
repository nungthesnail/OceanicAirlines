using EntityFrameworkLogic.Entities;

namespace BookingService.Models.BookingLogic
{
    /// <summary>
    /// Модель сущности пассажира
    /// </summary>
    public class PassengerModel
    {
        /// <summary>
        /// Идентификатор пассажира
        /// </summary>
        public int Id { get; set; }

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
        /// Номер документа, удостоверяющий личность пассажира
        /// </summary>
        public string DocumentNumber { get; set; } = null!;

        /// <summary>
        /// Гражданство пассажира
        /// </summary>
        public string DocumentIssuerCountry { get; set; } = null!;

        /// <summary>
        /// Дата рождения пассажира
        /// </summary>
        public DateOnly BirthDate { get; set; }

        /// <summary>
        /// Пол пассажира. 0 - М, 1 - Ж
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Номер телефона пассажира
        /// </summary>
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// Необязательный адрес электронной почты пассажира
        /// </summary>
        public string? Email { get; set; }

		/// <summary>
		/// 
		/// Метод, строящий модель пассажира из сущности пассажира. В будущем будет вынесен в отдельный строитель
		/// для разделения ответственности классов
		/// </summary>
		/// <param name="passenger">Сущность пассажира</param>
		/// <returns>Построенная модель пассажира</returns>
		public static PassengerModel BuildFrom(Passenger passenger)
        {
            return new PassengerModel
            {
                Id = passenger.Id,
                FirstName = passenger.FirstName,
                Surname = passenger.Surname,
                MiddleName = passenger.MiddleName,
                DocumentNumber = passenger.DocumentNumber,
                DocumentIssuerCountry = passenger.DocumentIssuerCountry,
                BirthDate = passenger.BirthDate,
                Gender = passenger.Gender,
                PhoneNumber = passenger.PhoneNumber,
                Email = passenger.Email
            };
        }
    }
}

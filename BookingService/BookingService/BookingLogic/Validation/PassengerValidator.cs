using EntityFrameworkLogic.Entities;
using BookingService.BookingLogic.Validation.Exceptions;
using System.Text.RegularExpressions;


namespace BookingService.BookingLogic.Validation
{
    /// <summary>
    /// Валидатор пассажира. Проверяет на корректность пассажира, готового к прикреплению к бронированию
    /// </summary>
    public static class PassengerValidator
    {
        /// <summary>
        /// Регулярное выражение для проверки коррекности ввода номера телефона
        /// </summary>
        private const string _phoneRegex = "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$";

		/// <summary>
		/// Проверяет на корректность пассажира, готового к прикреплению к бронированию
		/// </summary>
		/// <param name="passenger"></param>
		public static void Validate(Passenger passenger)
        {
            CheckBirthDate(passenger.BirthDate);

            CheckPhoneNumber(passenger.PhoneNumber);
        }

        private static void CheckBirthDate(DateOnly birthDate)
        {
            if (birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new PassengerWrongBirthDateException();
            }
        }

        private static void CheckPhoneNumber(string phoneNumber)
        {
            var phoneRegex = new Regex(_phoneRegex);

            if (!phoneRegex.IsMatch(phoneNumber))
            {
                throw new PassengerWrongPhoneNumberException();
            }
        }
    }
}

using FrontendService.Models.Booking;
using FrontendService.Models.Builders.Exceptions;
using InterserviceCommunication.Models.BookingService;


namespace FrontendService.Models.Builders
{
    /// <summary>
    /// Строитель модели бронирования для межсервисного взаимодействия
    /// </summary>
    public class InterserviceBookingModelBuilder
	{
        private int? _flightId = null;
        private Guid? _customerUserId = null;
        private IEnumerable<PassengerModel> _passengers = [];

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public InterserviceBookingModelBuilder()
        { }

		/// <summary>
		/// Устанавливает идентификатор запланированного рейса, на который совершается бронирование
		/// </summary>
		/// <param name="id">Идентификатор запланированного рейса, на который совершается бронирование</param>
		public void SetFlightId(int id)
        {
            _flightId = id;
        }

		/// <summary>
		/// Устанавливает уникальный идентификатор пользователя, совершающего бронирование
		/// </summary>
		/// <param name="id">Уникальный идентификатор пользователя, совершающего бронирование</param>
		public void SetCustomerUserId(Guid id)
        {
            _customerUserId = id;
        }

		/// <summary>
		/// Устанавливает список данных пассажиров бронирования
		/// </summary>
		/// <param name="passengers">Список данных пассажиров бронирования</param>
		public void SetPassengers(IEnumerable<PassengerModel> passengers)
        {
            _passengers = passengers;
        }

		/// <summary>
		/// Строит модель бронирования для межсервисного взаимодействия
		/// </summary>
		/// <returns>Построенная модель бронирования для межсервисного взаимодействия</returns>
		/// <exception cref="BuildingIsntCompletedException"></exception>
		public BookingServiceBookingModel Build()
        {
            ThrowIfBuildingIsntCompleted();

            var result = BuildInterserviceModel();

            return result;
        }

        private void ThrowIfBuildingIsntCompleted()
        {
            var flightIdSpecified = _flightId != null;
            var customerUserIdSpecified = _customerUserId != null;
            var passengersSpecified = _passengers.Any();

            var buildingCompleted = flightIdSpecified && customerUserIdSpecified && passengersSpecified;

            if (!buildingCompleted)
            {
                throw new BuildingIsntCompletedException();
            }
        }

        private BookingServiceBookingModel BuildInterserviceModel()
        {
            var passengers = from p in _passengers
                             select InterservicePassengerToBookingModelBuilder.BuildFrom(p);

            var result = new BookingServiceBookingModel()
            {
                FlightId = (int)_flightId!,
                CustomerUserId = (Guid)_customerUserId!,
                Confirmed = false,
                CreatedAt = DateTime.UtcNow,

                Passengers = passengers
            };

            return result;
        }

		/// <summary>
		/// Статический метод для построения модели бронирования для межсервисного взаимодействия
		/// </summary>
		/// <param name="flightId">Идентификатор запланированного рейса, на который совершается бронирование</param>
		/// <param name="customerUserId">Уникальный идентификатор пользователя, совершающего бронирование</param>
		/// <param name="passengers">Список данных пассажиров бронирования</param>
		/// <returns>Построенная модель бронирования для межсервисного взаимодействия</returns>
		/// <exception cref="BuildingIsntCompletedException"></exception>
		public static BookingServiceBookingModel Build(int flightId, Guid customerUserId, IEnumerable<PassengerModel> passengers)
        {
            var builder = new InterserviceBookingModelBuilder();

            builder.SetFlightId(flightId);
            builder.SetCustomerUserId(customerUserId);
            builder.SetPassengers(passengers);

            return builder.Build();
        }
    }
}

using EntityFrameworkLogic.Entities;
using InterserviceCommunication;
using InterserviceCommunication.Exceptions;
using BookingService.BookingLogic.Exceptions;
using InterserviceCommunication.Models.FlightService;
using BookingService.Repository;


namespace BookingService.BookingLogic.Validation
{
    /// <summary>
    /// Валидатор бронирования. Проверяет на корректность бронирование, готовое к добавлению в базу данных
    /// </summary>
    public class BookingValidator
    {
        private InterserviceCommunicator _interserviceCommunicator;

        private DatabasePassengerToBookingFacade _dbPassengerToBooking;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="interserviceCommunicator">Межсервисный связист</param>
        /// <param name="dbPassengersToBooking">Фасад базы данных</param>
        public BookingValidator(InterserviceCommunicator interserviceCommunicator, DatabasePassengerToBookingFacade dbPassengersToBooking)
        {
            _interserviceCommunicator = interserviceCommunicator;
            _dbPassengerToBooking = dbPassengersToBooking;
        }

        /// <summary>
        /// Проверяет корректность бронирования, готового к добавлению в базу данных
        /// </summary>
        /// <param name="booking">Бронирование</param>
        /// <param name="passengersCount">Количество пассажиров бронирования</param>
        /// <returns></returns>
        public async Task Validate(Booking booking, int passengersCount)
        {
            var flight = await GetFlightAndCheckExistence(booking.FlightId);

            CheckFlightDepartureTime(flight);

            await CheckCustomerUserExistence(booking.CustomerUserId);

            CheckNumberOfAvaibleSeats(flight, passengersCount);
        }

        private async Task<FlightServiceSheduledFlightModel> GetFlightAndCheckExistence(int flightId)
        {
            try
            {
                var flightServiceConnector = _interserviceCommunicator.GetFlightServiceConnector();
                var request = flightServiceConnector.CreateGetFlightRequest(flightId);
                var flight = await request.Send();

                return flight;
            }
            catch (NotFoundException)
            {
                throw new InvalidFlightException();
            }
        }

        private void CheckFlightDepartureTime(FlightServiceSheduledFlightModel flight)
        {
            if (flight.SheduledDeparture < DateTime.UtcNow)
            {
                throw new InvalidFlightException();
            }
        }

        private async Task CheckCustomerUserExistence(Guid userId)
        {
            var userServiceConnector = _interserviceCommunicator.GetUserServiceConnector();
            var request = userServiceConnector.CreateCheckUserExistsRequest(userId);
            var userExists = await request.Send();

            if (!userExists)
            {
                throw new InvalidCustomerUserException();
            }
        }

        private void CheckNumberOfAvaibleSeats(FlightServiceSheduledFlightModel flight, int passengersCount)
        {
            var flightSeatsCount = flight.SeatsCount;

            var allFlightPassengersAndBookings = _dbPassengerToBooking.GetAllForFlight(flight.Id);

            var alreadyHoldingSeatsCount = allFlightPassengersAndBookings.Count;

            var countOfSeatsMayBeHolding = alreadyHoldingSeatsCount + passengersCount;

            if (countOfSeatsMayBeHolding > flightSeatsCount)
            {
                throw new NotEnoughSeatsException();
            }
        }
    }
}

using BookingService.Repository;
using BookingService.Models.DbBuilders;
using EntityFrameworkLogic.Entities;
using BookingService.BookingLogic.Exceptions;
using BookingService.Models.BookingLogic;
using BookingService.Repository.Exceptions;
using BookingService.BookingLogic.Validation;


namespace BookingService.BookingLogic
{
    /// <summary>
    /// Сервис, реализующий логику создания бронирования
    /// </summary>
    public class BookingManagerService
    {
        private readonly DatabaseBookingFacade _dbBooking;

        private readonly DatabasePassengerFacade _dbPassenger;

        private readonly DatabasePassengerToBookingFacade _dbPassengerToBooking;

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="dbBooking">Фасад контекста базы данных (бронирования)</param>
        /// <param name="dbPassenger">Фасад контекста базы данных (пассажиры)</param>
        /// <param name="dbPassengerToBooking">Фасад контекста базы данных (пассажиры-к-бронированиям)</param>
        /// <param name="serviceProvider"></param>
        public BookingManagerService(
            DatabaseBookingFacade dbBooking,
            DatabasePassengerFacade dbPassenger,
            DatabasePassengerToBookingFacade dbPassengerToBooking,
            IServiceProvider serviceProvider)
        {
            _dbBooking = dbBooking;
            _dbPassenger = dbPassenger;
            _dbPassengerToBooking = dbPassengerToBooking;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Оформляет бронирование, предварительно проверяя его на корректность
        /// </summary>
        /// <param name="booking">Модель бронирования</param>
        /// <returns>Модель, отражающая созданное бронирование</returns>
        public async Task<BookingModel> Book(BookingModel booking)
        {
            var dbBooking = BuildBooking(booking);

            var dbPassengers = BuildPassengers(booking.Passengers);
                                                                   
            var dbPassengersToBookings = BuildPassengersToBookings(booking.Passengers);
                                                                                       
            var result = await Register(dbBooking, dbPassengers, dbPassengersToBookings);

            var response = CreateResponse(result);

            return response;
        }

        private Booking BuildBooking(BookingModel model)
        {
            return BookingBuilder.BuildFrom(model);
        }

        private List<Passenger> BuildPassengers(List<PassengerBookingModel> models)
        {
            var result = from pb in models
                         select PassengerBuilder.BuildFrom(pb.Passenger);

            return result.ToList();
        }

        private List<PassengerToBooking> BuildPassengersToBookings(List<PassengerBookingModel> models)
        {
            var result = from pb in models
                         select PassengerToBookingBuilder.BuildFrom(pb);

            return result.ToList();
        }

        private async Task<Booking> Register(Booking booking, List<Passenger> passengers, List<PassengerToBooking> passengersToBookings)
        {
            await ValidateBooking(booking, passengersToBookings.Count);

            var createdBooking = await CreateBooking(booking);

            await CreatePassengersAndPassengersToBookings(createdBooking, passengers, passengersToBookings);

            await SaveDb();

            var result = GetDbBookingById(createdBooking.Id);

            return result;
        }

        private async Task ValidateBooking(Booking booking, int passengersCount)
        {
            var bookingValidator = _serviceProvider.GetRequiredService<BookingValidator>();

            await bookingValidator.Validate(booking, passengersCount);
        }

        private async Task<Booking> CreateBooking(Booking booking)
        {
            return await _dbBooking.CreateNoSave(booking);
        }

        private async Task CreatePassengersAndPassengersToBookings(
            Booking parentBooking,
            List<Passenger> passengers,
            List<PassengerToBooking> passengersToBookings)
        {
            ThrowIfCountsArentEqual(passengers, passengersToBookings);

            for (int i = 0; i < passengers.Count(); ++i)
            {
                var passenger = passengers[i];
                var passengerToBooking = passengersToBookings[i];

                var createdPassenger = await CreatePassenger(passenger);

                await CreatePassengerToBooking(passengerToBooking, parentBooking, createdPassenger);
            }
        }

        private void ThrowIfCountsArentEqual(List<Passenger> a, List<PassengerToBooking> b)
        {
            var equal = (a.Count == b.Count);

            if (!equal)
            {
                RevertDbChanges();

                throw new WrongNumberOfEntitiesException("Number of passengers and links isn\'t equal");
            }
        }

        private void RevertDbChanges()
        {
            _dbBooking.RevertChanges();
            _dbPassenger.RevertChanges();
            _dbPassengerToBooking.RevertChanges();
        }

        private async Task<Passenger> CreatePassenger(Passenger passenger)
        {
            Passenger createdPassenger;

            try
            {
                createdPassenger = TryFindPassengerInDb(passenger);
            }
            catch (DoesntExistsException)
            {
                createdPassenger = await CreateNotFoundPassenger(passenger);
            }

            return createdPassenger;
        }

        private Passenger TryFindPassengerInDb(Passenger passenger)
        {
            var found = GetPassengerFromDbByDocumentNumber(passenger.DocumentNumber);

            ThrowIfProvidedDataDiffersFromDb(found, passenger); // TODO: действия в случае находки пассажира в базе данных, но данные в базе данных и запросе различаются

            return found;
        }

        private Passenger GetPassengerFromDbByDocumentNumber(string documentNumber)
        {
            return _dbPassenger.GetByDocumentNumber(documentNumber);
        }

        private void ThrowIfProvidedDataDiffersFromDb(Passenger provided, Passenger dbPassenger)
        {
            var equal = (
                provided.FirstName == dbPassenger.FirstName
                && provided.Surname == dbPassenger.Surname
                && provided.MiddleName == dbPassenger.MiddleName
                && provided.DocumentIssuerCountry == dbPassenger.DocumentIssuerCountry
                && provided.BirthDate == dbPassenger.BirthDate
                && provided.Gender == dbPassenger.Gender
            );

            if (!equal)
            {
                RevertDbChanges();

                throw new WrongPassengerDataException("The passenger was found in database but the provided data differs from found");
            }
        }

        private async Task<Passenger> CreateNotFoundPassenger(Passenger passenger)
        {
            ValidatePassengerBeforeCreation(passenger);

            return await CreatePassengerValidated(passenger);
        }

        private void ValidatePassengerBeforeCreation(Passenger passenger)
        {
            PassengerValidator.Validate(passenger);
        }

        private async Task<Passenger> CreatePassengerValidated(Passenger passenger)
        {
            return await _dbPassenger.CreateNoSave(passenger);
        }

        private async Task CreatePassengerToBooking(PassengerToBooking passengerToBooking, Booking parentBooking, Passenger passenger)
        {
            SetBookingAndPassengerKeys(ref passengerToBooking, parentBooking, passenger);

            ThrowIfPassengerToBookingAlreadyExists(passengerToBooking, parentBooking, passenger);

            await CreatePassengerToBookingValidated(passengerToBooking);
        }

        private void SetBookingAndPassengerKeys(ref PassengerToBooking passengerToBooking, Booking parentBooking, Passenger passenger)
        {
            passengerToBooking.Booking = parentBooking;
            passengerToBooking.Passenger = passenger;
        }

        private void ThrowIfPassengerToBookingAlreadyExists(PassengerToBooking passengerToBooking, Booking parentBooking, Passenger passenger)
        {
            var exists = _dbPassengerToBooking.Exists(parentBooking, passenger);

            if (exists)
            {
                throw new PassengerDuplicationException("Passenger already linked to booking");
            }
        }

        private async Task CreatePassengerToBookingValidated(PassengerToBooking passengerToBooking)
        {
            await _dbPassengerToBooking.CreateNoSave(passengerToBooking);
        }

        private async Task SaveDb()
        {
            try
            {
                await _dbBooking.Save();
                await _dbPassenger.Save();
                await _dbPassengerToBooking.Save();
            }
            catch (SavingFailedException exc)
            {
                RevertDbChanges();

                throw new BookingRegistrationException($"{exc}");
            }
        }

        private Booking GetDbBookingById(int id)
        {
            return _dbBooking.Get(id);
        }

        private BookingModel CreateResponse(Booking from)
        {
            return BookingModel.BuildFrom(from);
        }
    }
}

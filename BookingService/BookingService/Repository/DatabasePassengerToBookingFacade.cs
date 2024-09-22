using EntityFrameworkLogic.Entities;
using Microsoft.EntityFrameworkCore;
using BookingService.Repository.Exceptions;
using EntityFrameworkLogic;


namespace BookingService.Repository
{
	/// <summary>
	/// Фасад над контекстом базы данных, ответственный за управление сущностями связок пассажир-к-бронированию
	/// </summary>
	public class DatabasePassengerToBookingFacade : DatabaseFacade<PassengerToBooking>
    {
		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="applicationContext">Контекст базы данных</param>
		public DatabasePassengerToBookingFacade(ApplicationContext applicationContext)
            : base(applicationContext)
        { }

		/// <summary>
		/// Создает сущность связки в контексте базы данных, выбрасывая исключение в случае ошибки сохранения базы данных
		/// </summary>
		/// <param name="entity">Сущность связки</param>
		/// <returns>Созданная связка</returns>
		/// <exception cref="CreationFailedException"></exception>
		public override async Task<PassengerToBooking> Create(PassengerToBooking entity)
        {
            try
            {
                var entry = await _applicationContext.PassengersToBookings.AddAsync(entity);

                await _applicationContext.SaveChangesAsync();

                return entry.Entity;
            }
            catch (DbUpdateException)
            {
                throw new CreationFailedException();
            }
        }

		/// <summary>
		/// Создает сущность связки в контексте базы данных, не сохраняя изменения в базе данных
		/// </summary>
		/// <param name="entity">Сущность связки</param>
		/// <returns>Созданная сущность связки</returns>
		public override async Task<PassengerToBooking> CreateNoSave(PassengerToBooking entity)
        {
            var entry = await _applicationContext.PassengersToBookings.AddAsync(entity);

            return entry.Entity;
        }

		/// <summary>
		/// Получает сущность связки из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор связки</param>
		/// <returns>Найденная сущность связки</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override PassengerToBooking Get(int id)
        {
            var result = _applicationContext.PassengersToBookings
                                            .FirstOrDefault(p => p.Id == id);

            ThrowIfNullOrDefault(result);

            return result!;
        }

        private void ThrowIfNullOrDefault(PassengerToBooking? booking)
        {
            if (booking == null || booking == default)
            {
                throw new DoesntExistsException();
            }
        }

		/// <summary>
		/// Получает неотслеживаемую сущность связки из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор связки</param>
		/// <returns>Неотслеживаемая найденная сущность связки</returns>
		public override PassengerToBooking GetNoTracking(int id)
        {
            var result = _applicationContext.PassengersToBookings
                                            .AsNoTracking()
                                            .FirstOrDefault(p => p.Id == id);

            ThrowIfNullOrDefault(result);

            return result!;
        }

		/// <summary>
		/// Получает все сущности связок из контекста базы данных, бронирования которых оформлены на указанный запланированный рейс
		/// </summary>
		/// <param name="flightId"></param>
		/// <returns>Все сущности связок из контекста базы данных, бронирования которых оформлены на указанный запланированный рейс</returns>
		public List<PassengerToBooking> GetAllForFlight(int flightId)
        {
            return _applicationContext.PassengersToBookings
                                      .Where(x => x.Booking.FlightId == flightId)
                                      .ToList();
        }

		/// <summary>
		/// Получает все сущности связок из контекста базы данных
		/// </summary>
		/// <returns>Все сущности связок из контекста базы данных</returns>
		public override List<PassengerToBooking> GetAll()
        {
            return _applicationContext.PassengersToBookings.ToList();
        }

		/// <summary>
		/// Обновляет сущность связки в контексте базы данных и выбрасывает исключения в случае неудавшегося обновления
		/// </summary>
		/// <param name="entity">Сущность связки</param>
		/// <returns>Обновленная сущность связки</returns>
		/// <exception cref="UpdateFailedException"></exception>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<PassengerToBooking> Update(PassengerToBooking entity)
        {
            try
            {
                ThrowIfDoesntExists(entity.Id);

                var result = _applicationContext.PassengersToBookings.Update(entity);

                await _applicationContext.SaveChangesAsync();

                return result.Entity;
            }
            catch (DbUpdateException)
            {
                throw new UpdateFailedException();
            }
        }

        private void ThrowIfDoesntExists(int id)
        {
            if (!Exists(id))
            {
                throw new DoesntExistsException();
            }
        }

		/// <summary>
		/// Удаляет сущность связки из контекста базы данных, выбрасывая исключение при отсутствии сущности с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор связки</param>
		/// <returns>Удаленная сущность связки</returns>
		public override async Task<PassengerToBooking> Delete(int id)
        {
            ThrowIfDoesntExists(id);

            var entity = GetNoTracking(id);

            var result = _applicationContext.Remove(entity);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

		/// <summary>
		/// Проверяет существование сущности связки с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор связки</param>
		/// <returns>Существование связки</returns>
		public override bool Exists(int id)
        {
            return _applicationContext.PassengersToBookings
                                      .ToList()
                                      .Exists(x => x.Id == id);
        }

		/// <summary>
		/// Проверяет существование сущности связки с указанными идентификаторами пассажира и бронирования
		/// </summary>
		/// <param name="bookingId">Идентификатор бронирования</param>
		/// <param name="passengerId">Идентификатор пассажира</param>
		/// <returns>Существование связки</returns>
		public bool Exists(int bookingId, int passengerId)
        {
            return _applicationContext.PassengersToBookings
                                      .ToList()
                                      .Exists(x => x.PassengerId == passengerId
                                                && x.BookingId == bookingId);
        }

		/// <summary>
		/// Проверяет существование сущности связки с указанными сущностями пассажира и бронирования
		/// </summary>
		/// <param name="booking">Сущность бронирования</param>
		/// <param name="passenger">Сущность пассажира</param>
		/// <returns></returns>
		public bool Exists(Booking booking, Passenger passenger)
        {
            return _applicationContext.PassengersToBookings
                                      .ToList()
                                      .Exists(x => x.Passenger == passenger
                                                && x.Booking == booking);
        }

		/// <summary>
		/// Сохраняет изменения в базе данных и выбрасывает исключение в случае провала сохранения
		/// </summary>
		/// <returns></returns>
		/// <exception cref="SavingFailedException"></exception>
		public override async Task Save()
        {
            try
            {
                await _applicationContext.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                throw new SavingFailedException($"{exc}");
            }
        }

		/// <summary>
		/// Откатывает несохраненные изменения в контексте базы данных.
		/// </summary>
		public override void RevertChanges()
        {
            var changedEntries = _applicationContext.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged && x.Entity is PassengerToBooking).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}

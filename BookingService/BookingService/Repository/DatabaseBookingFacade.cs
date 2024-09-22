using EntityFrameworkLogic.Entities;
using Microsoft.EntityFrameworkCore;
using BookingService.Repository.Exceptions;
using EntityFrameworkLogic;
using BookingService.Services.BookingCodeGenerator;


namespace BookingService.Repository
{
	/// <summary>
	/// Фасад над контекстом базы данных, ответственный за управление сущностями бронирований
	/// </summary>
	public class DatabaseBookingFacade : DatabaseFacade<Booking>
    {
        private readonly IBookingCodeGeneratorService _codeGenerator;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="applicationContext">Контекст базы данных</param>
        /// <param name="codeGenerator">Сервис генерирования кода бронирования</param>
        public DatabaseBookingFacade(ApplicationContext applicationContext, IBookingCodeGeneratorService codeGenerator)
            : base(applicationContext)
        {
            _codeGenerator = codeGenerator;
            _codeGenerator.SetDatabaseBookingFacade(this);
        }

		/// <summary>
		/// Создает сущность бронирования в контексте базы данных, предварительно устанавливая нужные данные бронирования
		/// </summary>
		/// <param name="entity">Сущность бронирования</param>
		/// <returns>Созданная сущность бронирования</returns>
		/// <exception cref="CreationFailedException"></exception>
		public override async Task<Booking> Create(Booking entity)
        {
            try
            {
                SetBookingCreationTime(ref entity);

                SetBookingNotConfirmed(ref entity);

				GenerateBookingCode(ref entity);

				var entry = await _applicationContext.Bookings.AddAsync(entity);

                await _applicationContext.SaveChangesAsync();

                return entry.Entity;
            }
            catch (DbUpdateException)
            {
                throw new CreationFailedException();
            }
        }

		/// <summary>
		/// Создает сущность бронирования в контексте базы данных, не сохраняя изменения в базе данных,
        /// предварительно устанавливая нужные данные бронирования
		/// </summary>
		/// <param name="entity">Сущность бронирования</param>
		/// <returns>Созданная сущность бронирования</returns>
		public override async Task<Booking> CreateNoSave(Booking entity)
        {
            SetBookingCreationTime(ref entity);

            SetBookingNotConfirmed(ref entity);

            GenerateBookingCode(ref entity);

            var entry = await _applicationContext.Bookings.AddAsync(entity);

            return entry.Entity;
        }

        private void SetBookingCreationTime(ref Booking booking)
        {
            booking.CreatedAt = DateTime.UtcNow;
        }

        private void SetBookingNotConfirmed(ref Booking booking)
        {
            booking.Confirmed = false;
        }

        private void GenerateBookingCode(ref Booking booking)
        {
            booking.Code = _codeGenerator.Generate();
        }

		/// <summary>
		/// Получает сущность бронирования из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор бронирования</param>
		/// <returns>Найденная сущность бронирования</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override Booking Get(int id)
        {
            var result = _applicationContext.Bookings
											.Include(x => x.PassengersToBookings)
											.ThenInclude(y => y.Passenger)
											.FirstOrDefault(p => p.Id == id);

            ThrowIfNullOrDefault(result);

            return result!;
        }

        private void ThrowIfNullOrDefault(Booking? booking)
        {
            if (booking == null || booking == default)
            {
                throw new DoesntExistsException();
            }
        }

		/// <summary>
		/// Получает неотслеживаемую сущность бронирования из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор бронирования</param>
		/// <returns>Неотслеживаемая найденная сущность бронирования</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override Booking GetNoTracking(int id)
        {
            var result = _applicationContext.Bookings
                                            .AsNoTracking()
											.Include(x => x.PassengersToBookings)
                                            .ThenInclude(y => y.Passenger)
											.FirstOrDefault(p => p.Id == id);

            ThrowIfNullOrDefault(result);

            return result!;
        }

		/// <summary>
		/// Получает все сущности бронирований из контекста базы данных
		/// </summary>
		/// <returns>Все сущности бронирований из контекста базы данных</returns>
		public override List<Booking> GetAll()
        {
            return _applicationContext.Bookings.ToList();
        }

		/// <summary>
		/// Получает все сущности бронирований указанного пользователя из контекста базы данных
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Все найденные сущности бронирований указанного пользователя</returns>
		public List<Booking> GetByCustomerUserId(Guid userId)
        {
            var result = _applicationContext.Bookings
                                            .Include(x => x.PassengersToBookings)
											.ThenInclude(y => y.Passenger)
											.Where(x => x.CustomerUserId == userId)
                                            .ToList();

            return result;
        }

		/// <summary>
		/// Обновляет сущность бронирования в контексте базы данных и выбрасывает исключения в случае неудавшегося обновления
		/// </summary>
		/// <param name="entity">Сущность бронирования</param>
		/// <returns>Обновленная сущность бронирования</returns>
		/// <exception cref="UpdateFailedException"></exception>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<Booking> Update(Booking entity)
        {
            try
            {
                ThrowIfDoesntExists(entity.Id);

                var result = _applicationContext.Bookings.Update(entity);

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
		/// Удаляет сущность бронирования из контекста базы данных, выбрасывая исключение при отсутствии сущности с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор бронирования</param>
		/// <returns>Удаленная сущность бронирования</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<Booking> Delete(int id)
        {
            ThrowIfDoesntExists(id);

            var entity = GetNoTracking(id);

            var result = _applicationContext.Remove(entity);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

		/// <summary>
		/// Проверяет существование сущности бронирования с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор бронирования</param>
		/// <returns>Существование бронирования</returns>
		public override bool Exists(int id)
        {
            return _applicationContext.Bookings
                                      .ToList()
                                      .Exists(x => x.Id == id);
        }

		/// <summary>
		/// Проверяет существование сущности бронирования с указанным кодом бронирования
		/// </summary>
		/// <param name="bookingCode">Код бронирования</param>
		/// <returns>Существование бронирования</returns>
		public bool Exists(string bookingCode)
        {
            return _applicationContext.Bookings
                                      .ToList()
                                      .Exists(x => x.Code == bookingCode);
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
                .Where(x => x.State != EntityState.Unchanged && x.Entity is Booking).ToList();

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

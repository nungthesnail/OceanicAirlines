using EntityFrameworkLogic.Entities;
using Microsoft.EntityFrameworkCore;
using BookingService.Repository.Exceptions;
using EntityFrameworkLogic;


namespace BookingService.Repository
{
	/// <summary>
	/// Фасад над контекстом базы данных, ответственный за управление сущностями пассажиров
	/// </summary>
	public class DatabasePassengerFacade : DatabaseFacade<Passenger>
    {
        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="applicationContext">Контекст базы данных</param>
        public DatabasePassengerFacade(ApplicationContext applicationContext)
            : base(applicationContext)
        { }

		/// <summary>
		/// Создает сущность пассажира в контексте базы данных, выбрасывая исключение в случае ошибки сохранения базы данных
		/// </summary>
		/// <param name="entity">Сущность пассажира</param>
		/// <returns>Созданная сущность пассажира</returns>
		/// <exception cref="CreationFailedException"></exception>
		public override async Task<Passenger> Create(Passenger entity)
        {
            try
            {
                var entry = await _applicationContext.Passengers.AddAsync(entity);

                await _applicationContext.SaveChangesAsync();

                return entry.Entity;
            }
            catch (DbUpdateException)
            {
                throw new CreationFailedException();
            }
        }

		/// <summary>
		/// Создает сущность пассажира в контексте базы данных, не сохраняя изменения в базе данных
		/// </summary>
		/// <param name="entity">Сущность пассажира</param>
		/// <returns>Созданная сущность пассажира</returns>
		public override async Task<Passenger> CreateNoSave(Passenger entity)
        {
            var entry = await _applicationContext.Passengers.AddAsync(entity);

            return entry.Entity;
        }

		/// <summary>
		/// Получает сущность пассажира из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор пассажира</param>
		/// <returns>Найденная сущность пассажира</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override Passenger Get(int id)
        {
            var result = _applicationContext.Passengers
                                            .FirstOrDefault(p => p.Id == id);

            ThrowIfNullOrDefault(result);

            return result!;
        }

        private void ThrowIfNullOrDefault(Passenger? passenger)
        {
            if (passenger == null || passenger == default)
            {
                throw new DoesntExistsException();
            }
        }

		/// <summary>
		/// Получает неотслеживаемую сущность пассажира из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор пассажира</param>
		/// <returns>Неотслеживаемая найденная сущность пассажира</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override Passenger GetNoTracking(int id)
        {
            var result = _applicationContext.Passengers
                                            .AsNoTracking()
                                            .FirstOrDefault(p => p.Id == id);

            ThrowIfNullOrDefault(result);

            return result!;
        }

		/// <summary>
		/// Получает сущность пассажира из контекста базы данных по номеру документа удостоверяющего личность,
        /// выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="documentNumber">Номер документа удостоверяющего личность</param>
		/// <returns>Найденная сущность пассажира</returns>
		public Passenger GetByDocumentNumber(string documentNumber)
        {
            var result = _applicationContext.Passengers
                                            .FirstOrDefault(p => p.DocumentNumber == documentNumber);

            ThrowIfNullOrDefault(result);

            return result!;
        }

		/// <summary>
		/// Получает все сущности пассажиров из контекста базы данных
		/// </summary>
		/// <returns>Все сущности пассажиров из контекста базы данных</returns>
		public override List<Passenger> GetAll()
        {
            return _applicationContext.Passengers.ToList();
        }

		/// <summary>
		/// Обновляет сущность пассажира в контексте базы данных и выбрасывает исключения в случае неудавшегося обновления
		/// </summary>
		/// <param name="entity">Сущность пассажира</param>
		/// <returns>Обновленная сущность пассажира</returns>
		/// <exception cref="UpdateFailedException"></exception>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<Passenger> Update(Passenger entity)
        {
            try
            {
                ThrowIfDoesntExists(entity.Id);

                var result = _applicationContext.Passengers.Update(entity);

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
		/// Удаляет сущность пассажира из контекста базы данных, выбрасывая исключение при отсутствии сущности с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор сущности пассажира</param>
		/// <returns>Удаленная сущность бронирования</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<Passenger> Delete(int id)
        {
            ThrowIfDoesntExists(id);

            var entity = GetNoTracking(id);

            var result = _applicationContext.Remove(entity);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

		/// <summary>
		/// Проверяет существование сущности пассажира с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор пассажира</param>
		/// <returns>Существование сущности пассажира</returns>
		public override bool Exists(int id)
        {
            return _applicationContext.Passengers
                                      .ToList()
                                      .Exists(x => x.Id == id);
        }

		/// <summary>
		/// Проверяет существование сущности пассажира с указанным номером документа удостоверяющего личность пассажира
		/// </summary>
		/// <param name="documentNumber">Номер документа удостоверяющего личность</param>
		/// <returns>Существование сущности пассажира</returns>
		public bool Exists(string documentNumber)
        {
            return _applicationContext.Passengers
                                      .ToList()
                                      .Exists(x => x.DocumentNumber == documentNumber);
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
                .Where(x => x.State != EntityState.Unchanged && x.Entity is Passenger).ToList();

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

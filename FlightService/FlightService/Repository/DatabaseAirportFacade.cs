using EntityFrameworkLogic;
using EntityFrameworkLogic.Entities;
using FlightService.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace FlightService.Repository
{
    /// <summary>
    /// Фасад над контекстом базы данных, ответственный за управление сущностями аэропортов
    /// </summary>
    public class DatabaseAirportFacade : DatabaseFacade<Airport>
    {
        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="applicationContext">Контекст базы данных</param>
        public DatabaseAirportFacade(ApplicationContext applicationContext)
            : base(applicationContext)
        { }

        /// <summary>
        /// Создает сущность аэропорта в контексте базы данных, предварительно проверяя корректность данных создаваемой сущности
        /// </summary>
        /// <param name="airport">Сущность аэропорта</param>
        /// <returns>Созданная сущность</returns>
        /// <exception cref="CreationFailedException"></exception>
        public override async Task<Airport> Create(Airport airport)
        {
            try
            {
                ThrowIfExists(airport.CodeIata);

                var entry = await _applicationContext.Airports.AddAsync(airport);

                await _applicationContext.SaveChangesAsync();

                return entry.Entity;
            }
            catch (DbUpdateException)
            {
                throw new CreationFailedException();
            }
        }

        private void ThrowIfExists(string iataCode)
        {
            if (Exists(iataCode))
            {
                throw new AlreadyExistsException();
            }
        }

		/// <summary>
		/// Получает сущность аэропорта из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности аэропорта</param>
		/// <returns>Найденная сущность аэропорта</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override Airport Get(int id)
        {
            var result = _applicationContext.Airports
                                            .Where(x => x.Id == id)
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

        private void ThrowIfNullOrDefault(Airport? airport)
        {
            if (airport == null || airport == default)
            {
                throw new DoesntExistsException();
            }
        }

		/// <summary>
		/// Получает сущность аэропорта из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="iataCode">Код аэропорта IATA</param>
		/// <returns>Найденная сущность аэропорта</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public Airport GetByIataCode(string iataCode)
        {
            var result = _applicationContext.Airports
                                            .Where(x => x.CodeIata.ToUpper() == iataCode.ToUpper())
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

		/// <summary>
		/// Получает неотслеживаемую сущность аэропорта из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности аэропорта</param>
		/// <returns>Неотслеживаемая найденная сущность аэропорта</returns>
		public override Airport GetNoTracking(int id)
        {
            var result = _applicationContext.Airports
                                            .AsNoTracking()
                                            .Where(x => x.Id == id)
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

		/// <summary>
		/// Получает неотслеживаемую сущность аэропорта из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="iataCode">Код аэропорта IATA</param>
		/// <returns>Неотслеживаемая найденная сущность аэропорта</returns>
		public Airport GetByIataCodeNoTracking(string iataCode)
        {
            var result = _applicationContext.Airports
                                            .AsNoTracking()
                                            .Where(x => x.CodeIata == iataCode)
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

        /// <summary>
        /// Получает все сущности аэропортов из контекста базы данных
        /// </summary>
        /// <returns>Все сущности аэропортов из контекста базы данных</returns>
        public override List<Airport> GetAll()
        {
            return _applicationContext.Airports.ToList();
        }

		/// <summary>
		/// Обновляет сущность аэропорта в контексте базы данных и выбрасывает исключения в случае неудавшегося обновления
		/// </summary>
		/// <param name="entity">Сущность аэропорта</param>
		/// <returns>Обновленная сущность аэропорта</returns>
		/// <exception cref="UpdateFailedException"></exception>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<Airport> Update(Airport entity)
        {
            try
            {
                ThrowIfDoesntExists(entity.Id);

                var result = _applicationContext.Airports.Update(entity);

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
		/// Удаляет сущность аэропорта из контекста базы данных, выбрасывая исключение при отсутствии сущности с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор сущности аэропорта</param>
		/// <returns>Удаленная сущность аэропорта</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<Airport> Delete(int id)
        {
            ThrowIfDoesntExists(id);

            var entity = GetNoTracking(id);

            var result = _applicationContext.Remove(entity);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

		/// <summary>
		/// Удаляет сущность аэропорта из контекста базы данных, выбрасывая исключение при отсутствии сущности с указанным идентификатором
		/// </summary>
		/// <param name="iataCode">Код аэропорта IATA</param>
		/// <returns>Удаленная сущность аэропорта</returns>
		public async Task<Airport> Delete(string iataCode)
        {
            ThrowIfIataDoesntExists(iataCode);

            var entity = GetByIataCodeNoTracking(iataCode);

            var result = _applicationContext.Remove(entity);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

        private void ThrowIfIataDoesntExists(string iataCode)
        {
            if (!Exists(iataCode))
            {
                throw new DoesntExistsException();
            }
        }

        /// <summary>
        /// Проверяет существование сущности аэропорта с указанным идентификатором
        /// </summary>
        /// <param name="id">Идентификатор сущности аэропорта</param>
        /// <returns>Существование сущности аэропорта</returns>
        public override bool Exists(int id)
        {
            return _applicationContext.Airports
                                      .ToList()
                                      .Exists(x => x.Id == id);
        }

		/// <summary>
		/// Проверяет существование сущности аэропорта с указанным идентификатором
		/// </summary>
		/// <param name="iataCode">Код аэропорта IATA</param>
		/// <returns>Существование сущности аэропорта</returns>
		public bool Exists(string iataCode)
        {
            return _applicationContext.Airports
                                      .ToList()
                                      .Exists(x => x.CodeIata.ToUpper() == iataCode.ToUpper());
        }
    }
}

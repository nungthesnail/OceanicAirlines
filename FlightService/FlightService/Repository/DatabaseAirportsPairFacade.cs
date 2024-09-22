using EntityFrameworkLogic;
using EntityFrameworkLogic.Entities;
using FlightService.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace FlightService.Repository
{
	/// <summary>
	/// Фасад над контекстом базы данных, ответственный за управление сущностями пар аэропортов
	/// </summary>
	public class DatabaseAirportsPairFacade : DatabaseFacade<AirportsPair>
    {
		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="applicationContext">Контекст базы данных</param>
		public DatabaseAirportsPairFacade(ApplicationContext applicationContext)
            : base(applicationContext)
        { }

		/// <summary>
		/// Создает сущность пары аэропортов в контексте базы данных, предварительно проверяя корректность данных создаваемой сущности
		/// </summary>
		/// <param name="airportsPair"></param>
		/// <returns></returns>
		/// <exception cref="CreationFailedException"></exception>
		public override async Task<AirportsPair> Create(AirportsPair airportsPair)
        {
            try
            {
                var entry = await _applicationContext.AirportsPairs.AddAsync(airportsPair);

                await _applicationContext.SaveChangesAsync();

                return entry.Entity;
            }
            catch (DbUpdateException)
            {
                throw new CreationFailedException();
            }
        }

		/// <summary>
		/// Получает сущность пары аэропортов из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности пары аэропортов</param>
		/// <returns>Найденная сущность пары аэропортов</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override AirportsPair Get(int id)
        {
            var result = _applicationContext.AirportsPairs
                                            .Include(pair => pair.FirstAirport)
                                            .Include(pair => pair.SecondAirport)
                                            .Where(x => x.Id == id)
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

        private void ThrowIfNullOrDefault(AirportsPair? airport)
        {
            if (airport == null || airport == default)
            {
                throw new DoesntExistsException();
            }
        }

		/// <summary>
		/// Получает неотслеживаемую сущность пары аэропортов из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности пары аэропортов</param>
		/// <returns>Неотслеживаемая найденная сущность пары аэропортов</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override AirportsPair GetNoTracking(int id)
        {
            var result = _applicationContext.AirportsPairs
                                            .AsNoTracking()
											.Include(pair => pair.FirstAirport)
											.Include(pair => pair.SecondAirport)
											.Where(x => x.Id == id)
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

		/// <summary>
		/// Получает все сущности пар аэропортов из контекста базы данных
		/// </summary>
		/// <returns>Все сущности пар аэропортов из контекста базы данных</returns>
		public override List<AirportsPair> GetAll()
        {
            return _applicationContext.AirportsPairs
									  .Include(pair => pair.FirstAirport)
									  .Include(pair => pair.SecondAirport)
									  .ToList();
        }

		/// <summary>
		/// Обновляет сущность пары аэропортов в контексте базы данных и выбрасывает исключения в случае неудавшегося обновления
		/// </summary>
		/// <param name="entity">Сущность пары аэропортов</param>
		/// <returns>Обновленная сущность пары аэропортов</returns>
		/// <exception cref="UpdateFailedException"></exception>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<AirportsPair> Update(AirportsPair entity)
        {
            try
            {
                ThrowIfDoesntExists(entity.Id);

                var result = _applicationContext.AirportsPairs.Update(entity);

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
		/// Удаляет сущность пары аэропортов из контекста базы данных, выбрасывая исключение при отсутствии сущности с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор сущности пары аэропортов</param>
		/// <returns>Удаленная сущность пары аэропортов</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<AirportsPair> Delete(int id)
        {
            ThrowIfDoesntExists(id);

            var entity = GetNoTracking(id);

            var result = _applicationContext.Remove(entity);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

		/// <summary>
		/// Проверяет существование сущности пары аэропортов с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор сущности пары аэропортов</param>
		/// <returns>Существование сущности пары аэропортов</returns>
		public override bool Exists(int id)
        {
            return _applicationContext.AirportsPairs
                                      .ToList()
                                      .Exists(x => x.Id == id);
        }
    }
}

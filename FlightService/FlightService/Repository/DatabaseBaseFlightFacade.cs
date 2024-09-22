using EntityFrameworkLogic;
using EntityFrameworkLogic.Entities;
using FlightService.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace FlightService.Repository
{
	/// <summary>
	/// Фасад над контекстом базы данных, ответственный за управление сущностями базовых рейсов
	/// </summary>
	public class DatabaseBaseFlightFacade : DatabaseFacade<BaseFlight>
	{
		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="applicationContext">Контекст базы данных</param>
		public DatabaseBaseFlightFacade(ApplicationContext applicationContext)
            : base(applicationContext)
        { }

		/// <summary>
		/// Создает сущность базового рейса в контексте базы данных, предварительно проверяя корректность данных создаваемой сущности
		/// </summary>
		/// <param name="baseFlight">Сущность базового рейса</param>
		/// <returns>Созданная сущность базового рейса</returns>
		/// <exception cref="CreationFailedException"></exception>
		public override async Task<BaseFlight> Create(BaseFlight baseFlight)
        {
            try
            {
                var entry = await _applicationContext.BaseFlights.AddAsync(baseFlight);

                await _applicationContext.SaveChangesAsync();

                return entry.Entity;
            }
            catch (DbUpdateException)
            {
                throw new CreationFailedException();
            }
        }

		/// <summary>
		/// Получает сущность базового рейса из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности базового рейса</param>
		/// <returns>Найденная сущность базового рейса</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override BaseFlight Get(int id)
        {
            var result = _applicationContext.BaseFlights
                                            .Include(x => x.AirportsPair)
                                                .Include(x => x.AirportsPair.FirstAirport)
                                                .Include(x => x.AirportsPair.SecondAirport)
                                            .Where(x => x.Id == id)
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

        private void ThrowIfNullOrDefault(BaseFlight? airport)
        {
            if (airport == null || airport == default)
            {
                throw new DoesntExistsException();
            }
        }

		/// <summary>
		/// Получает неотслеживаемую сущность базового рейса из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности базового рейса</param>
		/// <returns>Неотслеживаемая найденная сущность базового рейса</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override BaseFlight GetNoTracking(int id)
        {
            var result = _applicationContext.BaseFlights
                                            .AsNoTracking()
											.Include(x => x.AirportsPair)
												.Include(x => x.AirportsPair.FirstAirport)
												.Include(x => x.AirportsPair.SecondAirport)
											.Where(x => x.Id == id)
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

		/// <summary>
		/// Получает все сущности базовых рейсов из контекста базы данных
		/// </summary>
		/// <returns>Все сущности базовых рейсов из контекста базы данных</returns>
		public override List<BaseFlight> GetAll()
        {
            return _applicationContext.BaseFlights
									  .Include(x => x.AirportsPair)
									      .Include(x => x.AirportsPair.FirstAirport)
									      .Include(x => x.AirportsPair.SecondAirport)
									  .ToList();
        }

		/// <summary>
		/// Обновляет сущность базового рейса в контексте базы данных и выбрасывает исключения в случае неудавшегося обновления
		/// </summary>
		/// <param name="entity">Сущность базового рейса</param>
		/// <returns>Обновленная сущность базового рейса</returns>
		/// <exception cref="UpdateFailedException"></exception>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<BaseFlight> Update(BaseFlight entity)
        {
            try
            {
                ThrowIfDoesntExists(entity.Id);

                var result = _applicationContext.BaseFlights.Update(entity);

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
		/// Удаляет сущность базового рейса из контекста базы данных, выбрасывая исключение при отсутствии сущности с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор сущности базового рейса</param>
		/// <returns>Удаленная сущность базового рейса</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<BaseFlight> Delete(int id)
        {
            ThrowIfDoesntExists(id);

            var entity = GetNoTracking(id);

            var result = _applicationContext.Remove(entity);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

		/// <summary>
		/// Проверяет существование сущности базового рейса с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор сущности базового рейса</param>
		/// <returns>Существование сущности базового рейса</returns>
		public override bool Exists(int id)
        {
            return _applicationContext.BaseFlights
                                      .ToList()
                                      .Exists(x => x.Id == id);
        }
    }
}

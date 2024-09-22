using EntityFrameworkLogic;
using EntityFrameworkLogic.Entities;
using FlightService.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace FlightService.Repository
{
	/// <summary>
	/// Фасад над контекстом базы данных, ответственный за управление сущностями базовых рейсов
	/// </summary>
	public class DatabaseSheduledFlightFacade : DatabaseFacade<SheduledFlight>
	{
		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="applicationContext">Контекст базы данных</param>
		public DatabaseSheduledFlightFacade(ApplicationContext applicationContext)
            : base(applicationContext)
        { }

		/// <summary>
		/// Создает сущность запланированного рейса в контексте базы данных, предварительно проверяя корректность данных создаваемой сущности
		/// </summary>
		/// <param name="sheduledFlight">Сущность запланированного рейса</param>
		/// <returns>Созданная сущность запланированного рейса</returns>
		/// <exception cref="CreationFailedException"></exception>
		public override async Task<SheduledFlight> Create(SheduledFlight sheduledFlight)
        {
            try
            {
                var entry = await _applicationContext.SheduledFlights.AddAsync(sheduledFlight);

                await _applicationContext.SaveChangesAsync();

                return entry.Entity;
            }
            catch (DbUpdateException)
            {
                throw new CreationFailedException();
            }
        }

		/// <summary>
		/// Получает сущность запланированного рейса из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности запланированного рейса</param>
		/// <returns>Найденная сущность запланированного рейса</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override SheduledFlight Get(int id)
        {
            var result = _applicationContext.SheduledFlights
                                            .Include(x => x.BaseFlight)
                                                .Include(x => x.BaseFlight.AirportsPair)
                                                    .Include(x => x.BaseFlight.AirportsPair.FirstAirport)
                                                    .Include(x => x.BaseFlight.AirportsPair.SecondAirport)
                                            .Where(x => x.Id == id)
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

        private void ThrowIfNullOrDefault(SheduledFlight? airport)
        {
            if (airport == null || airport == default)
            {
                throw new DoesntExistsException();
            }
        }

		/// <summary>
		/// Получает неотслеживаемую сущность запланированного рейса из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности запланированного рейса</param>
		/// <returns>Неотслеживаемая найденная сущность запланированного рейса</returns>
		public override SheduledFlight GetNoTracking(int id)
        {
            var result = _applicationContext.SheduledFlights
                                            .AsNoTracking()
											.Include(x => x.BaseFlight)
												.Include(x => x.BaseFlight.AirportsPair)
													.Include(x => x.BaseFlight.AirportsPair.FirstAirport)
													.Include(x => x.BaseFlight.AirportsPair.SecondAirport)
											.Where(x => x.Id == id)
                                            .FirstOrDefault();

            ThrowIfNullOrDefault(result);

            return result!;
        }

		/// <summary>
		/// Получает все сущности запланированных рейсов из контекста базы данных
		/// </summary>
		/// <returns>Все сущности запланированных рейсов из контекста базы данных</returns>
		public override List<SheduledFlight> GetAll()
        {
            return _applicationContext.SheduledFlights
									  .Include(x => x.BaseFlight)
										  .Include(x => x.BaseFlight.AirportsPair)
											  .Include(x => x.BaseFlight.AirportsPair.FirstAirport)
											  .Include(x => x.BaseFlight.AirportsPair.SecondAirport)
									  .ToList();
        }

		/// <summary>
		/// Обновляет сущность запланированного рейса в контексте базы данных и выбрасывает исключения в случае неудавшегося обновления
		/// </summary>
		/// <param name="entity">Сущность запланированного рейса</param>
		/// <returns>Обновленная сущность запланированного рейса</returns>
		/// <exception cref="UpdateFailedException"></exception>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<SheduledFlight> Update(SheduledFlight entity)
        {
            try
            {
                ThrowIfDoesntExists(entity.Id);

                var result = _applicationContext.SheduledFlights.Update(entity);

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
		/// Удаляет сущность запланированного рейса из контекста базы данных, выбрасывая исключение при отсутствии сущности с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор сущности запланированного рейса</param>
		/// <returns>Удаленная сущность запланированного рейса</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public override async Task<SheduledFlight> Delete(int id)
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
		/// <param name="id">Идентификатор сущности запланированного рейса</param>
		/// <returns>Существование сущности запланированного рейса</returns>
		public override bool Exists(int id)
        {
            return _applicationContext.SheduledFlights
                                      .ToList()
                                      .Exists(x => x.Id == id);
        }
    }
}

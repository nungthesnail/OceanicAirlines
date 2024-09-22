using BookingService.Repository.Exceptions;
using EntityFrameworkLogic;


namespace BookingService.Repository
{
	/// <summary>
	/// Фасад над контекстом базы данных, управляющий определенными типами сущностей
	/// </summary>
	/// <typeparam name="TEntity">Тип сущности</typeparam>
	public abstract class DatabaseFacade<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext _applicationContext;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="applicationContext">Контекст базы данных</param>
        public DatabaseFacade(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

		/// <summary>
		/// Создает сущность в контексте базы данных
		/// </summary>
		/// <param name="entity">Сущность</param>
		/// <returns>Созданная сущность</returns>
		public abstract Task<TEntity> Create(TEntity entity);

		/// <summary>
		/// Создает сущность в контексте базы данных, не производя сохранение базы данных
		/// </summary>
		/// <param name="entity">Сущность</param>
		/// <returns>Созданная сущность</returns>
		public abstract Task<TEntity> CreateNoSave(TEntity entity);

		/// <summary>
		/// Получает сущность из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности</param>
		/// <returns>Найденная сущность</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public abstract TEntity Get(int id);

		/// <summary>
		/// Получает неотслеживаемую сущность из контекста базы данных, выбрасывая исключение при отсутствии сущности
		/// </summary>
		/// <param name="id">Идентификатор сущности</param>
		/// <returns>Неотслеживаемая найденная сущность</returns>
		public abstract TEntity GetNoTracking(int id);

		/// <summary>
		/// Получает все сущности из контекста базы данных
		/// </summary>
		/// <returns>Все сущности из контекста базы данных</returns>
		public abstract List<TEntity> GetAll();

		/// <summary>
		/// Обновляет сущность в контексте базы данных и выбрасывает исключения в случае неудавшегося обновления
		/// </summary>
		/// <param name="entity">Сущность</param>
		/// <returns>Обновленная сущность</returns>
		/// <exception cref="UpdateFailedException"></exception>
		/// <exception cref="DoesntExistsException"></exception>
		public abstract Task<TEntity> Update(TEntity entity);

		/// <summary>
		/// Удаляет сущность из контекста базы данных, выбрасывая исключение при отсутствии сущности с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор сущности</param>
		/// <returns>Удаленная сущность</returns>
		/// <exception cref="DoesntExistsException"></exception>
		public abstract Task<TEntity> Delete(int id);

		/// <summary>
		/// Проверяет существование сущности с указанным идентификатором
		/// </summary>
		/// <param name="id">Идентификатор сущности</param>
		/// <returns>Существование сущности</returns>
		public abstract bool Exists(int id);

		/// <summary>
		/// Сохраняет изменения в базе данных и выбрасывает исключение в случае провала сохранения
		/// </summary>
		/// <returns></returns>
		/// <exception cref="SavingFailedException"></exception>
		public abstract Task Save();

		/// <summary>
		/// Откатывает несохраненные изменения в контексте базы данных.
		/// </summary>
		public abstract void RevertChanges();
    }
}

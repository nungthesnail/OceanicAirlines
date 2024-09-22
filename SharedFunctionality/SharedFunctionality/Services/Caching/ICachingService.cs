
namespace SharedFunctionality.Services.Caching
{
	/// <summary>
	/// Интерфейс сервиса кэширования
	/// </summary>
	public interface ICachingService
	{
		/// <summary>
		/// Получить значение из кэша
		/// </summary>
		/// <typeparam name="TResult">Тип значения</typeparam>
		/// <param name="key">Ключ</param>
		/// <returns>Значение из кэша</returns>
		/// <exception cref="NullReferenceException"></exception>
		public Task<TResult?> Get<TResult>(string key);

		/// <summary>
		/// Получить значение из кэша с заданным префиксом ключа
		/// </summary>
		/// <typeparam name="TPrefix">Тип значения префикса</typeparam>
		/// <typeparam name="TResult">Тип значения</typeparam>
		/// <param name="prefix">Префикс ключа</param>
		/// <param name="key">Ключ</param>
		/// <returns>Значение из кэша</returns>
		/// <exception cref="NullReferenceException"></exception>
		public Task<TResult?> GetWithPrefix<TPrefix, TResult>(TPrefix prefix, string key);

		/// <summary>
		/// Установить значение в кэше
		/// </summary>
		/// <typeparam name="TValue">Тип значения</typeparam>
		/// <param name="key">Ключ</param>
		/// <param name="value">Значение</param>
		/// <returns></returns>
		public Task Set<TValue>(string key, TValue value);

		/// <summary>
		/// Установить значение в кэше с заданным префиксом ключа
		/// </summary>
		/// <typeparam name="TPrefix">Тип значения префикса</typeparam>
		/// <typeparam name="TValue">Тип значения</typeparam>
		/// <param name="prefix">Префикс ключа</param>
		/// <param name="key">Ключ</param>
		/// <param name="value">Значение</param>
		/// <returns></returns>
		public Task SetWithPrefix<TPrefix, TValue>(TPrefix prefix, string key, TValue value);

		/// <summary>
		/// Удаляет значение из кэша
		/// </summary>
		/// <param name="key">Ключ</param>
		/// <returns></returns>
		public Task Delete(string key);

		/// <summary>
		/// Удаляет значение из кэша с заданным префиксом ключа
		/// </summary>
		/// <typeparam name="TPrefix">Тип значения префикса</typeparam>
		/// <param name="prefix">Знаячение префикса ключа</param>
		/// <param name="key">Ключ</param>
		/// <returns></returns>
		public Task DeleteWithPrefix<TPrefix>(TPrefix prefix, string key);
	}
}
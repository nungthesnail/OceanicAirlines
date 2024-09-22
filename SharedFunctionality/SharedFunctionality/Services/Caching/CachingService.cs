using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;


namespace SharedFunctionality.Services.Caching
{
	/// <summary>
	/// Сервис кеширования Redis
	/// </summary>
	public class CachingService : ICachingService
	{
		private readonly JsonSerializerOptions _jsonOptions;

		private readonly IDistributedCache _cache = null!;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="cache">Распределенный кэш</param>
		public CachingService(IDistributedCache cache)
		{
			_cache = cache;

			_jsonOptions = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true
			};
		}

		public async Task Set<T>(string key, T value)
		{
			var serialized = Serialize(value);

			var cacheOptions = CreateCacheOptions();

			await _cache.SetStringAsync(key, serialized, cacheOptions);
		}

		private DistributedCacheEntryOptions CreateCacheOptions()
		{
			return new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
			};
		}

		public async Task<TResult?> Get<TResult>(string key)
		{
			var cached = await _cache.GetStringAsync(key);

			ThrowIfNull(cached);

			return Deserialize<TResult>(cached!);
		}

		private void ThrowIfNull(object? value)
		{
			if (value == null)
			{
				throw new NullReferenceException();
			}
		}

		public async Task Delete(string key)
		{
			await _cache.RemoveAsync(key);
		}

		public async Task SetWithPrefix<TPrefix, TValue>(TPrefix prefix, string key, TValue value)
		{
			var prefixedKey = $"{prefix}-{key}";

			await Set(prefixedKey, value);
		}

		public async Task<TResult?> GetWithPrefix<TPrefix, TResult>(TPrefix prefix, string key)
		{
			var prefixedKey = $"{prefix}-{key}";

			return await Get<TResult>(prefixedKey);
		}

		public async Task DeleteWithPrefix<TPrefix>(TPrefix prefix, string key)
		{
			var prefixedKey = $"{prefix}-{key}";

			await Delete(prefixedKey);
		}

		private TResult? Deserialize<TResult>(string json)
		{
			var formatted = FormatIfRequired(typeof(TResult), json);

			return JsonSerializer.Deserialize<TResult>(json, _jsonOptions);
		}

		private string FormatIfRequired(Type resultType, string json)
		{
			var result = json;

			if (resultType != typeof(string))
			{
				result = TrimDoubleQuotas(result);
			}
			
			return json;
		}

		private string TrimDoubleQuotas(string json)
		{
			return json.Trim('\"');
		}

		private string Serialize<T>(T value)
		{
			return JsonSerializer.Serialize(value, _jsonOptions);
		}
	}
}

using BuisnessLogic.Models.Management;
using BuisnessLogic.Repository.Exceptions;
using EntityFrameworkLogic;
using Microsoft.EntityFrameworkCore;

namespace BuisnessLogic.Repository
{
    /// <summary>
    /// Класс репозитория базы данных. Фасад над контекстом базы данных
    /// </summary>
    public class RepositoryFacade
    {
        private readonly DbPasswordHashBuilder _passwordHashBuilder;

        private readonly ApplicationContext _applicationContext;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="applicationContext">Контекст базы данных</param>
        /// <param name="passwordHashBuilder">Строитель сущности хеша пароля пользователя</param>
        public RepositoryFacade(ApplicationContext applicationContext, DbPasswordHashBuilder passwordHashBuilder)
        {
            _passwordHashBuilder = passwordHashBuilder;
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Метод создания хеша пароля пользователя в базе данных. Проверяет коррекность запроса и делегирует обращение к базе данных контексту
        /// </summary>
        /// <param name="request">Модель пароля пользователя</param>
        /// <returns>Модель хеша пароля пользователя</returns>
        public async Task<ManagementResponse> Create(ManagementRequest request)
        {
            var passwordHash = _passwordHashBuilder.BuildFrom(request);
            CreateNewPasswordHashId(ref passwordHash);

            ThrowIfUserLinked(passwordHash.LinkedUserId);

            var createdPasswordHash = await CreateDbPasswordHash(passwordHash);

            return ConvertDbPasswordHashToManagementResponse(createdPasswordHash);
        }

        private void CreateNewPasswordHashId(ref PasswordHash passwordHash)
        {
            passwordHash.Id = Guid.NewGuid();
        }

        private void ThrowIfPasswordHashIdExists(Guid id)
        {
            if (PasswordHashIdExists(id))
            {
                throw new PasswordHashAlreadyExistsException();
            }
        }

        private bool PasswordHashIdExists(Guid id)
        {
            return _applicationContext.PasswordHashes
                                      .AsNoTracking()
                                      .ToList()
                                      .Exists(x => x.Id == id);
        }

        private void ThrowIfUserLinked(Guid userId)
        {
            if (UserLinked(userId))
            {
                throw new UserAlreadyLinkedException();
            }
        }

        /// <summary>
        /// Метод проверки существования пароля у пользователя. Делегирует обращение к базе данных контексту
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns>Существование пароля у пользователя</returns>
        public bool UserLinked(Guid userId)
        {
            return _applicationContext.PasswordHashes
                                      .AsNoTracking()
                                      .ToList()
                                      .Exists(x => x.LinkedUserId == userId);
        }

        private async Task<PasswordHash> CreateDbPasswordHash(PasswordHash passwordHash)
        {
            var result = await _applicationContext.PasswordHashes
                                                  .AddAsync(passwordHash);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

        private ManagementResponse ConvertDbPasswordHashToManagementResponse(PasswordHash passwordHash)
        {
            return new ManagementResponse(
                passwordHash.Id,
                passwordHash.LinkedUserId,
                passwordHash.HashedPassword);
        }

        /// <summary>
        /// Метод получения хеша пароля пользователя по уникальному идентификатору хеша пароля. Проверяет корректность и делегирует обращение к базе данных контексту
        /// </summary>
        /// <param name="id">Уникальный идентификатор хеша пароля</param>
        /// <returns>Хеш пароля пользователя</returns>
        public ManagementResponse GetById(Guid id)
        {
            ThrowIfPasswordHashIdDoesntExists(id);

            var result = GetDbPasswordHashById(id);

            return ConvertDbPasswordHashToManagementResponse(result);
        }

        private void ThrowIfPasswordHashIdDoesntExists(Guid id)
        {
            if (!PasswordHashIdExists(id))
            {
                throw new PasswordHashDoesntExistsException();
            }
        }

        private PasswordHash GetDbPasswordHashById(Guid id)
        {
            return _applicationContext.PasswordHashes
                                      .AsNoTracking()
                                      .Where(x => x.Id == id)
                                      .First();
        }

        private PasswordHash GetDbPasswordHashByIdTracking(Guid id)
        {
            return _applicationContext.PasswordHashes
                                      .Where(x => x.Id == id)
                                      .First();
        }

        /// <summary>
        /// Метод получения хеша пароля пользователя по уникальному идентификатору пользователя. Проверяет корректность и делегирует контексту обращение к базе данных
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ManagementResponse GetByLinkedUserId(Guid userId)
        {
            ThrowIfUserDoesntLinked(userId);

            var result = GetDbPasswordHashByLinkedUserId(userId);

            return ConvertDbPasswordHashToManagementResponse(result);
        }

        private void ThrowIfUserDoesntLinked(Guid userId)
        {
            if (!UserLinked(userId))
            {
                throw new UserDoesntLinkedException();
            }
        }

        private PasswordHash GetDbPasswordHashByLinkedUserId(Guid userId)
        {
            return _applicationContext.PasswordHashes
                                      .AsNoTracking()
                                      .Where(x => x.LinkedUserId == userId)
                                      .First();
        }

        private PasswordHash GetDbPasswordHashByLinkedUserIdTracking(Guid userId)
        {
            return _applicationContext.PasswordHashes
                                      .Where(x => x.LinkedUserId == userId)
                                      .First();
        }

        /// <summary>
        /// Метод обновления хеша пароля пользователя в базе данных. Проверяет корректность и делегирует контексту обращение к базе данных
        /// </summary>
        /// <param name="request">Модель пароля пользователя</param>
        /// <returns>Обновленный хеш пароля пользователя</returns>
        public async Task<ManagementResponse> Update(ManagementRequest request)
        {
            var passwordHash = _passwordHashBuilder.BuildFrom(request);

            ThrowIfPasswordHashIdDoesntExists(passwordHash.Id);

            var dbEntry = GetDbPasswordHashByIdTracking(passwordHash.Id);

            CopyPasswordHashFields(ref passwordHash, ref dbEntry);

            var result = await UpdateDbPasswordHash(dbEntry);

            return ConvertDbPasswordHashToManagementResponse(result);
        }

        private void CopyPasswordHashFields(ref PasswordHash from, ref PasswordHash to)
        {
            to.HashedPassword = from.HashedPassword;
        }

        private async Task<PasswordHash> UpdateDbPasswordHash(PasswordHash passwordHash)
        {
            var result = _applicationContext.Update(passwordHash)
                                            .Entity;

            await _applicationContext.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Метод удаления хеша пароля пользователя по уникальному идентификатору хеша пароля. Проверяет корректность и делегирует обращение контексту обращение к базе данных
        /// </summary>
        /// <param name="id">Уникальный идентификатор хеша пароля пользователя</param>
        /// <returns>Удаленный хеш пароля пользователя</returns>
        public async Task<ManagementResponse> DeleteById(Guid id)
        {
            ThrowIfPasswordHashIdDoesntExists(id);

            var passwordHash = GetDbPasswordHashById(id);

            var deleted = await DeleteDbPasswordHash(passwordHash);

            return ConvertDbPasswordHashToManagementResponse(deleted);
        }

		/// <summary>
		/// Метод удаления хеша пароля пользователя по уникальному идентификатору пользователя. Проверяет корректность и делегирует обращение контексту обращение к базе данных
		/// </summary>
		/// <param name="id">Уникальный идентификатор пользователя</param>
		/// <returns>Удаленный хеш пароля пользователя</returns>
		public async Task<ManagementResponse> DeleteByLinkedUserId(Guid id)
        {
            ThrowIfUserDoesntLinked(id);

            var passwordHash = GetDbPasswordHashByLinkedUserId(id);

            var deleted = await DeleteDbPasswordHash(passwordHash);

            return ConvertDbPasswordHashToManagementResponse(deleted);
        }

        private async Task<PasswordHash> DeleteDbPasswordHash(PasswordHash passwordHash)
        {
            var result = _applicationContext.PasswordHashes
                                            .Remove(passwordHash)
                                            .Entity;

            await _applicationContext.SaveChangesAsync();

            return result;
        }
    }
}

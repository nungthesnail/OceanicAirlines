using BuisnessLogic.Models;
using EntityFrameworkLogic;
using BuisnessLogic.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace BuisnessLogic.Repository
{
    /// <summary>
    /// Репозиторий. Фасад для работы с контекстом базы данных
    /// </summary>
    public class RepositoryFacade
    {
        private ApplicationContext _applicationContext;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="applicationContext">Контекст базы данных</param>
        public RepositoryFacade(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Метод создания пользователя, добавляющий необходимые данные и делегирующий создание контексту базы данных
        /// </summary>
        /// <param name="requestUserModel">Модель пользователя из запроса</param>
        /// <returns>Созданный пользователь</returns>
        public async Task<ResponseUserModel> Create(RequestUserModel requestUserModel)
        {
            RequestUserModelCreateGuid(ref requestUserModel);

            var dbUser = CreateDbUserModel(requestUserModel);

            SetCreationDateTime(dbUser);

            await AddToContext(dbUser);

            return GetById(requestUserModel.Id);
        }

        private void RequestUserModelCreateGuid(ref RequestUserModel user)
        {
            user.Id = Guid.NewGuid();
        }

        private User CreateDbUserModel(RequestUserModel source)
        {
            return DbUserModelBuilder.FromRequestUserModel(source);
        }

        private void SetCreationDateTime(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
        }

        private async Task AddToContext(User user)
        {
            _applicationContext.Users.Add(user);

            await _applicationContext.SaveChangesAsync();
        }

        /// <summary>
        /// Метод получения пользователя, делегирующий получение контексту базы данных
        /// </summary>
        /// <param name="id">Уникальный идентификатор пользователя</param>
        /// <returns>Найденный пользователь</returns>
        public ResponseUserModel GetById(Guid id)
        {
            var dbUser = GetDbUserById(id);

            var converted = ConvertDbUserToResponseUserModel(dbUser);

            return converted;
        }

        private User GetDbUserById(Guid id)
        {
            var result = _applicationContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == id);

            CheckDbUserValid(result);

            return result!;
        }

        private User GetDbUserByIdTracking(Guid id)
        {
            var result = _applicationContext.Users.FirstOrDefault(x => x.Id == id);

            CheckDbUserValid(result);

            return result!;
        }

		/// <summary>
		/// Метод получения пользователя, делегирующий получение контексту базы данных
		/// </summary>
		/// <param name="name">Имя пользователя</param>
		/// <returns>Найденный пользователь</returns>
		public ResponseUserModel GetByName(string name)
        {
            var dbUser = GetDbUserByName(name);

            var converted = ConvertDbUserToResponseUserModel(dbUser);

            return converted;
        }

        private User GetDbUserByName(string name)
        {
            var result = _applicationContext.Users.AsNoTracking().FirstOrDefault(x => x.Name == name);

            CheckDbUserValid(result);

            return result!;
        }

		/// <summary>
		/// Метод получения пользователя, делегирующий получение контексту базы данных
		/// </summary>
		/// <param name="email">Адрес электронной почты пользователя</param>
		/// <returns>Найденный пользователь</returns>
		public ResponseUserModel GetByEmail(string email)
        {
            var dbUser = GetDbUserByEmail(email);

            var converted = ConvertDbUserToResponseUserModel(dbUser);

            return converted;
        }

        private User GetDbUserByEmail(string email)
        {
            var result = _applicationContext.Users.AsNoTracking().FirstOrDefault(x => x.Email == email);

            CheckDbUserValid(result);

            return result!;
        }

        /// <summary>
        /// Метод обновления пользователя, делегирующий обновление контексту базы данных
        /// </summary>
        /// <param name="requestUserModel">Модель пользователя</param>
        /// <returns>Обновленный пользователь</returns>
        public async Task<ResponseUserModel> Update(RequestUserModel requestUserModel)
        {
            var dbUser = CreateDbUserModel(requestUserModel);

            dbUser = await UpdateDbUser(dbUser);

            return ConvertDbUserToResponseUserModel(dbUser);
        }

        private async Task<User> UpdateDbUser(User dbUser)
        {
            var trackingEntity = GetDbUserByIdTracking(dbUser.Id);

            CopyDbUserFields(dbUser, ref trackingEntity);

            var result = _applicationContext.Update(trackingEntity).Entity;

            await _applicationContext.SaveChangesAsync();

            return result;
        }

        private void CopyDbUserFields(User from, ref User to)
        {
            to.Name = from.Name;
            to.Email = from.Email;
        }

        /// <summary>
        /// Метод удаления пользователя, делегирующий удаление контексту базы данных
        /// </summary>
        /// <param name="id">Уникальный идентификатор пользователя</param>
        /// <returns>Удаленный пользователь</returns>
        public async Task<ResponseUserModel> DeleteById(Guid id)
        {
            var dbUser = PrepareDbUserFromId(id);

            var entity = await DeleteDbUser(dbUser);

            return ConvertDbUserToResponseUserModel(entity);
        }

        private User PrepareDbUserFromId(Guid id)
        {
            var user = GetById(id);

            var dbUser = DbUserModelBuilder.FromResponseUserModel(user);

            return dbUser;
        }

		/// <summary>
		/// Метод удаления пользователя, делегирующий удаление контексту базы данных
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Удаленный пользователь</returns>
		public async Task<ResponseUserModel> DeleteByName(string userName)
        {
            var dbUser = PrepareDbUserFromName(userName);

            var entity = await DeleteDbUser(dbUser);

            return ConvertDbUserToResponseUserModel(entity);
        }

        private User PrepareDbUserFromName(string userName)
        {
            var user = GetByName(userName);

            var dbUser = DbUserModelBuilder.FromResponseUserModel(user);

            return dbUser;
        }

        private async Task<User> DeleteDbUser(User dbUser)
        {
            var entity = _applicationContext.Remove(dbUser).Entity;

            await _applicationContext.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// Метод проверки существования пользователя
        /// </summary>
        /// <param name="id">Уникальный идентификатор пользователя</param>
        /// <returns>Существование пользователя</returns>
        public bool UserExists(Guid id)
        {
            return _applicationContext.Users.ToList().Exists(x => x.Id == id);
        }

        /// <summary>
        /// Метод проверки существования пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Существование пользователя</returns>
        public bool UserExists(string userName)
        {
            return _applicationContext.Users.ToList().Exists(x => x.Name == userName);
        }

        private ResponseUserModel ConvertDbUserToResponseUserModel(User user)
        {
            return DbUserModelBuilder.DbUserModelToResponseUserModel(user);
        }

        private void CheckDbUserValid(User? user)
        {
            if (user == default || user == null)
            {
                throw new UserNotFoundException();
            }
        }
    }
}

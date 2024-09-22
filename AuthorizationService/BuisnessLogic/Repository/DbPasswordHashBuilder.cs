using BuisnessLogic.Models.Management;
using BuisnessLogic.Repository.Exceptions;
using EntityFrameworkLogic;
using PasswordUtils;

namespace BuisnessLogic.Repository
{
    /// <summary>
    /// Строитель сущности хеша пароля пользователя из базы данных
    /// </summary>
    public class DbPasswordHashBuilder
    {
        private Guid _id;

        private Guid _linkedUserId;

        private string? _hashedPassword;

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public DbPasswordHashBuilder()
        { }

        /// <summary>
        /// Метод построения сущности хеша пароля пользователя
        /// </summary>
        /// <returns>Сущность хеша пароля пользователя</returns>
        public PasswordHash Build()
        {
            ThrowExceptionIfIsntConfigured();

            var result = new PasswordHash
            {
                Id = _id,
                LinkedUserId = _linkedUserId,
                HashedPassword = _hashedPassword!
            };

            return result;
        }

        private void ThrowExceptionIfIsntConfigured()
        {
            if (_hashedPassword == null)
            {
                throw new PasswordHashBuilderIsntConfiguredException();
            }
        }

        /// <summary>
        /// Метод построения сущности хеша пароля пользователя из модели пароля пользователя
        /// </summary>
        /// <param name="request">Модель пароля пользователя</param>
        /// <returns></returns>
        public PasswordHash BuildFrom(ManagementRequest request)
        {
            _id = request.Id;
            _linkedUserId = request.LinkedUserId;
            _hashedPassword = GeneratePasswordHash(request.Password);

            return Build();
        }

        private string GeneratePasswordHash(string password)
        {
            return new PasswordHasher().Generate(password);
        }

        /// <summary>
        /// Метод построения сущности хеша пароля пользователя из модели хеша пароля пользователя
        /// </summary>
        /// <param name="response">Модель хеша пароля пользователя</param>
        /// <returns>Сущность хеша пароля пользователя</returns>
        public PasswordHash BuildFrom(ManagementResponse response)
        {
            _id = response.Id;
            _linkedUserId = response.LinkedUserId;
            _hashedPassword = response.HashedPassword;

            return Build();
        }

        /// <summary>
        /// Устанавливает уникальный идентификатор хеша пароля пользователя
        /// </summary>
        /// <param name="id">Уникальный идентификатор хеша пароля пользователя</param>
        public void SetId(Guid id)
        {
            _id = id;
        }

        /// <summary>
        /// Устанавливает уникальный идентификатор связанного пользователя
        /// </summary>
        /// <param name="id">Уникальный идентификатор связанного пользователя</param>
        public void SetLinkedUserId(Guid id)
        {
            _linkedUserId = id;
        }

        /// <summary>
        /// Устанавливает хеш пароля пользователя
        /// </summary>
        /// <param name="hashedPassword">Хеш пароля пользователя</param>
        public void SetHashedPassword(string hashedPassword)
        {
            _hashedPassword = hashedPassword;
        }

        /// <summary>
        /// Устанавливает хеш пароля пользователя, сгенерированный из пароля пользователя
        /// </summary>
        /// <param name="password">Пароль пользователя</param>
        public void SetPassword(string password)
        {
            _hashedPassword = GeneratePasswordHash(password);
        }
    }
}

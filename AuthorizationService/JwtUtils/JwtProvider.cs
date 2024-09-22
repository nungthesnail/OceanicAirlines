using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtUtils
{
    /// <summary>
    /// Провайдер JSON Web Token'ов
    /// </summary>
    public class JwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// Конструктор провайдера
        /// </summary>
        /// <param name="jwtOptions">Настройки провайдера</param>
		public JwtProvider(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        /// <summary>
        /// Генерирует JSON Web Token
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="userRole">Роль пользователя</param>
        /// <param name="expires">Срок истечения токена</param>
        /// <returns>Сгенерированный JSON Web Token</returns>
        public string GenerateToken(Guid userId, string userName, string userRole, DateTime expires = default)
        {
            SetDefaultExpireTimeIfDefault(ref expires);

            var claims = CreateClaims(userId, userName, userRole);
            var credentials = CreateSigningCredentials();

            var token = CreateJwtSecurityToken(claims, credentials, expires);
            var tokenString = ConvertTokenToString(token);

            return tokenString;
        }

        private void SetDefaultExpireTimeIfDefault(ref DateTime expires)
        {
            if (expires == default)
            {
                expires = DateTime.UtcNow.AddHours(1);
            }
        }

        private Claim[] CreateClaims(Guid userId, string userName, string userRole)
        {
            return
            [
                new Claim("UserId", userId.ToString()),
                new Claim("UserName", userName),
                new Claim(ClaimTypes.Role, userRole)
            ];
        }

        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials
            (
                new SymmetricSecurityKey
                (
                    Encoding.UTF8.GetBytes(_jwtOptions.Secret)
                ),
                SecurityAlgorithms.HmacSha256
            );
        }

        private JwtSecurityToken CreateJwtSecurityToken(
            Claim[] claims, SigningCredentials credentials, DateTime expires)
        {
            return new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: expires,
                issuer: "AuthenticationService"
            );
        }

        private string ConvertTokenToString(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

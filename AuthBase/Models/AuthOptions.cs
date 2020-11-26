using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthBase.Models
{
    public class AuthOptions
    {
        public const string Security = "Security";
        
        public string Audience { get; set; }

        public string Issuer { get; set; }
        
        public string Secret { get; set; }
        
        /// <summary>
        /// Значение, превысив которое врменно блокируется попытка авторизации пользователя.
        /// Максимальное кол-во попыток входа
        /// </summary>
        public int MaxAuthFailedCount { get ; set; }

        /// <summary>
        /// Длительность активной сессии для пользователя. В минутах
        /// </summary>
        public int? SessionLifeTime { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
        }
    }
}
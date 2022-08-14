using System.IdentityModel.Tokens.Jwt;
using System.ComponentModel.DataAnnotations;

namespace TimeManager.IdP.Authentication
{
    public class Token
    {
        public Token(){}
        public Token(string jwtToken)
        {
            token = jwtToken;
        }

        [Key]
        public int Id { get; set; }
        public string token { get; set; }
        public DateTime createDate { get; set; }
        public DateTime expirationDate { get; set; }
        public int userId { get; set; }

    }

    public class TokenKey
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
    }
}

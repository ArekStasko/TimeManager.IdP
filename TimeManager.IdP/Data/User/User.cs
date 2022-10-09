using System.ComponentModel.DataAnnotations;

namespace TimeManager.IdP.Data
{
    public class User : IUser
    {
        public User(string userName, byte[] passwordHash, byte[] passwordSalt)
        {
            UserName = userName;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Token { get; set; }
        public string? RefreshTokens { get; set; }

    }
}

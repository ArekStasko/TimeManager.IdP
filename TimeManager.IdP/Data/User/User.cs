using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeManager.IdP.Data
{
    public class User : IUser
    {
        public User() {}
        public User(string userName, byte[] passwordHash, byte[] passwordSalt)
        {
            UserName = userName;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Token { get; set; }

    }
}

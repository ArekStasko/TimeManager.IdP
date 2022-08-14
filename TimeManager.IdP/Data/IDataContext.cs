using Microsoft.EntityFrameworkCore;
using TimeManager.IdP.Authentication;

namespace TimeManager.IdP.Data
{
    public interface IDataContext
    {
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenKey> TokenKey { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

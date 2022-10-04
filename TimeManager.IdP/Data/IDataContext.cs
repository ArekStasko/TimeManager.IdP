using Microsoft.EntityFrameworkCore;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Data
{
    public interface IDataContext
    {
        public DbSet<TokenKey> TokenKey { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

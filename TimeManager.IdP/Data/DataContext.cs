using Microsoft.EntityFrameworkCore;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<TokenKey> TokenKey { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

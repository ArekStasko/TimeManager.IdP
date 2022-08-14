using Microsoft.EntityFrameworkCore;
using TimeManager.IdP.Authentication;

namespace TimeManager.IdP.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenKey> TokenKey { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

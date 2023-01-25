using Microsoft.EntityFrameworkCore;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<TokenKey> TokenKey { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}

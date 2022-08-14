using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TimeManager.IdP.Data;

namespace TimeManager.IdP.Data.Services
{
    public static class DatabaseManagerService
    {

        public static void MigrationInitialization(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<DataContext>().Database.Migrate();
            }
        }
    }
}

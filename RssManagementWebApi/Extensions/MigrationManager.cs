using Microsoft.EntityFrameworkCore;
using RssManagementWebApi.DB;

namespace RssManagementWebApi.Extensions;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost webHost)
    {
        using (var scope = webHost.Services.CreateScope())
        {
            using var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                appContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return webHost;
    }
}
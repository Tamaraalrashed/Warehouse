using Core;
using Data;
using Microsoft.EntityFrameworkCore;

namespace REST
{

    public static class SqliteExtensions
    {
        public static IApplicationBuilder UseSqliteMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();

            return app;
        }
    }

}

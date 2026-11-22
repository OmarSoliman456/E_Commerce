using E_Commerce.Domain.Contract;
using System.Security.Principal;

namespace E_Commerce.API.Extensions
{
    public static class WebApplicationsExtensions
    {
        public static async Task<WebApplication> SeedAndMigrateDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope(); 

            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            var Identityseeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");

            await seeder.SeedDataAsync();
            await Identityseeder.SeedDataAsync();
            return app;
        }
    }
}

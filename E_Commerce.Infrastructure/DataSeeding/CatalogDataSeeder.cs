using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class CatalogDataSeeder(StoreDbContext dbContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);

                if (pendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync(ct);
                }

                var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");

                await SeedIfEmptyAsync<ProductBrand, int>(seedRoot, "brands.json", ct);

                await SeedIfEmptyAsync<ProductType, int>(seedRoot, "types.json", ct);

                await SeedIfEmptyAsync<Product, int>(seedRoot, "products.json", ct);

                await SeedIfEmptyAsync<DeliveryMethod, int>(seedRoot, "delivery.json", ct);

                int result = await dbContext.SaveChangesAsync(ct);

                if (result > 0)
                {
                    logger.LogInformation(
                        $"{result} Rows Inserted");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
            }
        }


        private async Task SeedIfEmptyAsync<T, TKey>(string rootPath, string fileName, CancellationToken ct) where T : BaseEntity<TKey>
        {
            if (await dbContext.Set<T>().AnyAsync(ct))
            {
                logger.LogInformation("Table Already Has Data");
                return;
            }

            // Build file path
            var filePath = Path.Combine(rootPath, fileName);

            // Check if file exists
            if (!File.Exists(filePath))
            {
                logger.LogWarning($"{fileName} Does Not Exist");
                return;
            }

            // Open JSON file
            using var fileStream = File.OpenRead(filePath);

            // JSON options
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserialize JSON
            var items =
                await JsonSerializer.DeserializeAsync<List<T>>(fileStream, options, ct);
            // Add data to DbContext
            if (items?.Any() ?? false)
            {
                dbContext.Set<T>().AddRange(items);
            }
        }
    }
}

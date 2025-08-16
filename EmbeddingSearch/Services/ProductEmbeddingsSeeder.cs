using EmbeddingSearch.Data;
using Microsoft.EntityFrameworkCore;

namespace EmbeddingSearch.Services
{
    public static class ProductEmbeddingsSeeder
    {
        public static async Task SeedProductEmbeddings(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var embeddingService = serviceProvider.GetRequiredService<EmbeddingService>();
            var logger = serviceProvider.GetRequiredService<ILogger>();

            var products = await context.Products
                .Where(p => p.Embeddings == null)
                .ToListAsync();

            if (products.Count == 0)
            {
                return; // No products to seed
            }

            foreach (var product in products)
            {
                try
                {
                    var content = $"{product.Name} {product.Description}";
                    var embeddings = await embeddingService.GetEmbeddingsAsync(content);
                    product.Embeddings = embeddings;
                    // Update the product in the database
                    context.Products.Update(product);
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Error generating embeddings for product {product.Id}: {ex.Message}");
                }
            }

            // Save all changes to the database
            try
            {
                await context.SaveChangesAsync();
                logger.LogInformation("Product embeddings seeded successfully.");
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Error saving product embeddings: {ex.Message}");
            }
        }
    }
}

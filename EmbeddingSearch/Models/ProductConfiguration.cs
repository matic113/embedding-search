using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmbeddingSearch.Models
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasIndex(p => new { p.Name, p.Description })
                .HasMethod("gin")
                .IsTsVectorExpressionIndex("english");

            builder.Property(p => p.Embeddings)
                .HasColumnType("vector(1536)")
                .HasPrecision(1536) // Adjust precision based on your embedding size
                .HasComment("Embeddings for product search");

            builder.HasIndex(p => p.Embeddings)
                .HasMethod("hnsw")
                .HasOperators("vector_cosine_ops")
                .HasStorageParameter("m", 16)
                .HasStorageParameter("ef_construction", 64);
        }
    }
}

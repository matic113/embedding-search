using Pgvector;

namespace EmbeddingSearch.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Vector? Embeddings { get; set; }
    }
}

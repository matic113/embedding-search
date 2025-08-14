namespace EmbeddingSearch.Controllers
{
    public class CreateProductRequest
    {
        public required string ProductName { get; set; }
        public required string ProductDescription { get; set; }
    }
}
namespace EmbeddingSearch.Controllers
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Distance { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EmbeddingSearch.Controllers
{
    public class CreateProductRequest
    {
        [Required]
        public required string ProductName { get; set; }
        [Required]
        public required string ProductDescription { get; set; }
    }
}
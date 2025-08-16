using System.ComponentModel.DataAnnotations;

namespace EmbeddingSearch.Services
{
    public class AzureEmbeddingOptions
    {
        [Required]
        public required string DeploymentName { get; set; }
        [Required]
        [Url]
        public required string Endpoint { get; set; }
        [Required]
        public required string Key { get; set; }
    }
}

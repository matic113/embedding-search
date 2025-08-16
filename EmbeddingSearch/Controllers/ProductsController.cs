using EmbeddingSearch.Data;
using EmbeddingSearch.Models;
using EmbeddingSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace EmbeddingSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmbeddingService _embeddingService;

        public ProductsController(ApplicationDbContext context, EmbeddingService embeddingService)
        {
            _context = context;
            _embeddingService = embeddingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Provide a valid product");
            }

            var product = new Product { Name = request.ProductName, Description = request.ProductDescription };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("search/fts")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty");
            }

            var products = await _context.Products
                .Where(p => EF.Functions.ToTsVector("english", p.Name + " " + p.Description)
                    .Matches(EF.Functions.PhraseToTsQuery(query)))
                .ToListAsync();
            return Ok(products);
        }

        [HttpGet("search/embeddings")]
        public async Task<IActionResult> SearchProductsByEmbeddings(string query, int limit = 5)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty");
            }

            var queryVector = await _embeddingService.GetEmbeddingsAsync(query);

            if (queryVector is null)
            {
                return BadRequest("Failed to generate embeddings for the query");
            }

            var products = await _context.Products
                .OrderBy(p => p.Embeddings!.CosineDistance(queryVector))
                .Take(limit)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description
                })
                .ToListAsync();

            return Ok(products);
        }
    }
}

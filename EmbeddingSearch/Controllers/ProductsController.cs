using EmbeddingSearch.Data;
using EmbeddingSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmbeddingSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
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
                    .Matches(EF.Functions.PlainToTsQuery(query)))
                .ToListAsync();
            return Ok(products);
        }
    }
}

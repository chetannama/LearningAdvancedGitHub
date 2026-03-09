using EmailSpamDetectionService.DTOs;
using EmailSpamDetectionService.Helpers;
using EmailSpamDetectionService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmailSpamDetectionService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EmailSpamDbContext _context;
        private readonly ILogger<ProductController> _logger;
        public ProductController(EmailSpamDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("/add-product")]
        public async Task<IActionResult> Add(ProductDTO productDto)
        {
            if (productDto != null)
            {
                var product = new Product
                {
                    //Id = 123,
                    Name = productDto.Name,
                    Price = productDto.Price,
                };
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product Name- {product.Name} and Price- {product.Price}", product.Id);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            //var requestETag = Request.Headers["If-Match"].ToString().Replace("\"", "");

            //if (string.IsNullOrEmpty(requestETag))
            //    return BadRequest("ETag is required.");

            //var currentETag = Convert.ToBase64String(product.RowVersion);

            //if (requestETag != currentETag)
            //    return StatusCode(412, "Resource modified by another user.");

            product.Name = dto.Name;
            product.Price = dto.Price;

            //Optimistic Concurrency
            _context.Entry(product)
        .Property(p => p.RowVersion)
        .OriginalValue = dto.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product updated successfully for productId: {id}", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError($"DbUpdateConcurrencyException occured for product id: {id}",id);
                return Conflict("This record was modified by another user.");
            }

            //await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            //_logger.LogInformation("CorrelationId is {CorrelationId}", correlationId);
            _logger.LogInformation("Fetching product {ProductId}", id);
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning($"product details is empty for productId: {id}", id);
                return NotFound();
            }

            var etag = Convert.ToBase64String(product.RowVersion);

            _logger.LogInformation($"ETag: {etag} for the product id:{id}", id);
            Response.Headers["ETag"] = $"\"{etag}\"";

            var dto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                RowVersion = product.RowVersion
            };
            _logger.LogInformation($"Get product details for productId: {id}", id);
            return Ok(dto);
        }
    }
}

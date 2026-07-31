using Microsoft.AspNetCore.Mvc;
using ProductSphere.Application.DTOs.ProductDtos;
using ProductSphere.Application.Interfaces.IServices;

namespace ProductSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        private readonly ILogger<ProductController> _logger;

        //using DI
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            _logger.LogInformation("GetAll Products API called.");
            throw new Exception("Test exception");
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            await _productService.CreateAsync(createProductDto);
            return Ok("Product created successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProductDto updateProductDto)
        {
            await _productService.UpdateAsync(updateProductDto);
            return Ok("Product updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok("Product deleted successfully.");
        }
    }
}
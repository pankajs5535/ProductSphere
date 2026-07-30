using Microsoft.AspNetCore.Mvc;
using ProductSphere.Application.DTOs.ItemDtos;
using ProductSphere.Application.Interfaces.IServices;

namespace ProductSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _itemService.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
        {
            await _itemService.CreateAsync(dto);
            return Ok(new
            {
                Message = "Item created successfully."
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateItemDto dto)
        {
            await _itemService.UpdateAsync(dto);
            return Ok(new
            {
                Message = "Item updated successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemService.DeleteAsync(id);
            return Ok(new
            {
                Message = "Item deleted successfully."
            });
        }
    }
}
using Catalog.Service.DTO;
using Catalog.Service.Entities;
using Catalog.Service.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Service.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private readonly ItemRepository itemRepository = new();
        //     private static readonly List<ItemDto> items = new(){
        //         new ItemDto(Guid.NewGuid(),"Potion","Restore a small amount of HP",5,DateTimeOffset.UtcNow),
        //         new ItemDto(Guid.NewGuid(),"Antidote","Cures poision",7,DateTimeOffset.UtcNow),
        //         new ItemDto(Guid.NewGuid(),"Bronze Sword","Deal small amount of damage",20,DateTimeOffset.UtcNow)
        // };
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemRepository.GetAllAsync()).Select(items => items.AsDto());
            return items;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemByIdAsync(Guid id)
        {
            //var item = items.Where(item => item.Id == id).SingleOrDefault();
            var item = await itemRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }



        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(CreateDto createItemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await itemRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetItemByIdAsync), new { id = item.Id }, item);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, UpdateDto updateDto)
        {
            var existingItem = await itemRepository.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updateDto.Name;
            existingItem.Description = updateDto.Description;
            existingItem.Price = updateDto.Price;
            await itemRepository.UpdateAsync(existingItem);
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            // var index = items.FindIndex(existingItem => existingItem.Id == id);
            var existingItem = await itemRepository.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            itemRepository.DeleteAsync(existingItem.Id);
            return NoContent();
        }
    }
}
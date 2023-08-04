using Catalog.Service.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Service.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private static readonly List<ItemDto> items = new(){
            new ItemDto(Guid.NewGuid(),"Potion","Restore a small amount of HP",5,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Antidote","Cures poision",7,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Bronze Sword","Deal small amount of damage",20,DateTimeOffset.UtcNow)
    };
        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }


        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItemById(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }



        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }




        [HttpPut("{id}")]
        public IActionResult UpdateItem(Guid id,UpdateDto updateDto)
        {
            var existingItem = items.Where(items => items.Id == id).FirstOrDefault();
            if (existingItem == null)
            {
                return NotFound();
            }
            var updatedItem = existingItem with
            {
                Name = updateDto.Name,
                Price = updateDto.Price,
                Description = updateDto.Description
            };
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updatedItem;
            return NoContent();
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteItem(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            if (index < 0)
            {
                return NotFound();
            }
            items.RemoveAt(index);
            return NoContent();
        }
    }
}
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository ItemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            ItemsRepository = itemsRepository;
        }

        /// <summary>
        /// GET api/items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            IEnumerable<Item> Items = await ItemsRepository.GetItemsAsync();
            return Items.Select(item => item.AsDto());
        }

        /// <summary>
        /// GET api/items/{id}.
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            Item ItemToReturn = await ItemsRepository.GetItemAsync(id);

            if (ItemToReturn is null)
            {
                return NotFound();
            }

            return ItemToReturn.AsDto();
        }

        /// <summary>
        /// POST api/items
        /// </summary>
        /// <param name="itemDto">Item data.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item Item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.Now
            };

            await ItemsRepository.CreateItemAsync(Item);

            return CreatedAtAction(nameof(GetItemAsync), new { id = Item.Id }, Item.AsDto());
        }

        /// <summary>
        /// PUT api/items/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemDto">Item data.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            Item ExistingItem = await ItemsRepository.GetItemAsync(id);

            if (ExistingItem is null)
            {
                return NotFound();
            }

            // Create a copy of the existing item with the following modifications.
            Item UpdatedItem = ExistingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await ItemsRepository.UpdateItemAsync(UpdatedItem);

            return NoContent();
        }

        /// <summary>
        /// DELETE api/items/{id}
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            Item ExistingItem = await ItemsRepository.GetItemAsync(id);

            if (ExistingItem is null)
            {
                return NotFound();
            }

            await ItemsRepository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}
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
        public IEnumerable<ItemDto> GetItems()
        {
            IEnumerable<ItemDto> items = ItemsRepository.GetItems().Select(item => item.AsDto());
            return items;
        }

        /// <summary>
        /// GET api/items/{id}.
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            Item ItemToReturn = ItemsRepository.GetItem(id);

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
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item Item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.Now
            };

            ItemsRepository.CreateItem(Item);

            return CreatedAtAction(nameof(GetItem), new { id = Item.Id }, Item.AsDto());
        }

        /// <summary>
        /// PUT api/items/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemDto">Item data.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            Item ExistingItem = ItemsRepository.GetItem(id);

            if (ExistingItem is null)
            {
                return NotFound();
            }

            Item UpdatedItem = ExistingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            ItemsRepository.UpdateItem(UpdatedItem);

            return NoContent();
        }

        /// <summary>
        /// DELETE api/items/{id}
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            Item ExistingItem = ItemsRepository.GetItem(id);

            if (ExistingItem is null)
            {
                return NotFound();
            }

            ItemsRepository.DeleteItem(id);

            return NoContent();
        }
    }
}
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
        /// <param name="id"></param>
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
    }
}
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
        public IEnumerable<Item> GetItems()
        {
            return ItemsRepository.GetItems();
        }

        /// <summary>
        /// GET api/items/{id}.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {
            Item ItemToReturn = ItemsRepository.GetItem(id);

            if (ItemToReturn is null)
            {
                return NotFound();
            }

            return ItemsRepository.GetItem(id);
        }
    }
}
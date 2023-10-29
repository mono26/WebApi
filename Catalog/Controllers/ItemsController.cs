using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly InMemItemsRepository ItemsRepository;

        public ItemsController()
        {
            ItemsRepository = new InMemItemsRepository();
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
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public Item GetItem(Guid Id)
        {
            return ItemsRepository.GetItem(Id);
        }
    }
}
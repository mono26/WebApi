using Catalog.Entities;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        public IEnumerable<Item> GetItems();
        public Item GetItem(Guid id);
        public void CreateItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(Guid id);
    }
}
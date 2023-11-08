using Catalog.Entities;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        public IEnumerable<Item> GetItems();
        public Item GetItem(Guid Id);
        public void CreateItem(Item item);
        void UpdateItem(Item item);
    }
}
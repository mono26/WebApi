using Catalog.Entities;

namespace Catalog.Repositories
{
    public class InMemItemsRepository : IItemsRepository
    {
        private readonly List<Item> Items = new()
        {
            new Item() { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow },
            new Item() { Id = Guid.NewGuid(), Name = "Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
            new Item() { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate = DateTimeOffset.UtcNow }
        };

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(Items);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            Item ExistingItem = Items.Where((Item) => Item.Id == id).SingleOrDefault();
            return await Task.FromResult(ExistingItem);
        }

        public async Task CreateItemAsync(Item item)
        {
            Items.Add(item);
            await Task.CompletedTask;
        }

        public async Task UpdateItemAsync(Item item)
        {
            int index = Items.FindIndex((existingItem) => existingItem.Id == item.Id);
            Items[index] = item;
            await Task.CompletedTask;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            int index = Items.FindIndex((existingItem) => existingItem.Id == id);
            Items.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}
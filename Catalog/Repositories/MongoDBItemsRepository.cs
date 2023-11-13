using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        private const string DB_NAME = "catalog";
        private const string COLLECTION_NAME = "items";

        private readonly IMongoCollection<Item> ItemsCollection;
        private readonly FilterDefinitionBuilder<Item> FilterBuilder = Builders<Item>.Filter;

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase Database = mongoClient.GetDatabase(DB_NAME);
            ItemsCollection = Database.GetCollection<Item>(COLLECTION_NAME);
        }

        public async Task CreateItemAsync(Item item)
        {
            await ItemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            FilterDefinition<Item> Filter = FilterBuilder.Eq((item) => item.Id, id);
            await ItemsCollection.DeleteOneAsync(Filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            FilterDefinition<Item> Filter = FilterBuilder.Eq((item) => item.Id, id);
            return await ItemsCollection.Find(Filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await ItemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            FilterDefinition<Item> Filter = FilterBuilder.Eq((existingItem) => existingItem.Id, item.Id);
            await ItemsCollection.ReplaceOneAsync(Filter, item);
        }
    }
}
using System.Data.Common;
using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDBItemsRepository : IItemsRepository
    {
        private const string DB_NAME = "catalog";
        private const string COLLECTION_NAME = "items";

        private readonly IMongoCollection<Item> ItemsCollection;
        private readonly FilterDefinitionBuilder<Item> FilterBuilder = Builders<Item>.Filter;

        public MongoDBItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase Database = mongoClient.GetDatabase(DB_NAME);
            ItemsCollection = Database.GetCollection<Item>(COLLECTION_NAME);
        }

        public void CreateItem(Item item)
        {
            ItemsCollection.InsertOne(item);
        }

        public void DeleteItem(Guid id)
        {
            FilterDefinition<Item> Filter = FilterBuilder.Eq((item) => item.Id, id);
            ItemsCollection.DeleteOne(Filter);
        }

        public Item GetItem(Guid id)
        {
            FilterDefinition<Item> Filter = FilterBuilder.Eq((item) => item.Id, id);
            return ItemsCollection.Find(Filter).SingleOrDefault();
        }

        public IEnumerable<Item> GetItems()
        {
            return ItemsCollection.Find(new BsonDocument()).ToList();
        }

        public void UpdateItem(Item item)
        {
            FilterDefinition<Item> Filter = FilterBuilder.Eq((existingItem) => existingItem.Id, item.Id);
            ItemsCollection.ReplaceOne(Filter, item);
        }
    }
}
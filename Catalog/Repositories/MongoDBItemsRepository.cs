using System.Data.Common;
using Catalog.Entities;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDBItemsRepository : IItemsRepository
    {
        private const string DB_NAME = "catalog";
        private const string COLLECTION_NAME = "items";

        private readonly IMongoCollection<Item> ItemsCollection;

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
            throw new NotImplementedException();
        }

        public Item GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetItems()
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
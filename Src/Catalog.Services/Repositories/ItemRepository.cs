using Catalog.Service.Entities;
using MongoDB.Driver;

namespace Catalog.Service.Repositories
{
    public class ItemRepository
    {
        private const string collectionName = "Items";
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            await dbCollection.InsertOneAsync(entity);
        }
        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
        public async Task DeleteAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, id);
            if (filter == null)
            { 
                throw new ArgumentNullException();
            }

            await dbCollection.DeleteOneAsync(filter);
        }
    }
}
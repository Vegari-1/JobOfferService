using MongoDB.Bson;
using MongoDB.Driver;

namespace JobOfferService.IntegrationTests
{
    public static class DbExtensions
    {
        public static string InsertJobOffer(this IntegrationWebApplicationFactory<Program> factory,
            string positionName, string description)
        {
            MongoClient dbClient = new MongoClient(factory.mongoContainer.ConnectionString);
            var database = dbClient.GetDatabase(factory.DATABASE_NAME);
            var collection = database.GetCollection<BsonDocument>(factory.COLLECTION_NAME);
            BsonDocument document = new BsonDocument
            {
                { "PositionName", positionName },
                { "Description", description }
            };
            collection.InsertOne(document);
            return document["_id"].AsObjectId.ToString();
        }

        public static void DeleteJobOfferById(this IntegrationWebApplicationFactory<Program> factory,
                string id)
        {
            MongoClient dbClient = new MongoClient(factory.mongoContainer.ConnectionString);
            var database = dbClient.GetDatabase(factory.DATABASE_NAME);
            var collection = database.GetCollection<BsonDocument>(factory.COLLECTION_NAME);
            collection.DeleteOne(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id)));
        }
    }
}

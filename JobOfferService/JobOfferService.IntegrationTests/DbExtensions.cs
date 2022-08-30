using System;

using MongoDB.Bson;
using MongoDB.Driver;

using JobOfferService.Model;

namespace JobOfferService.IntegrationTests;

public static class DbExtensions
{
    public static string InsertJobOffer(this IntegrationWebApplicationFactory<Program> factory,
        string positionName, string description, string companyLink, string[] qualifications)
    {
        MongoClient dbClient = new MongoClient(factory.mongoContainer.ConnectionString);
        var database = dbClient.GetDatabase(factory.DATABASE_NAME);
        var collection = database.GetCollection<BsonDocument>(factory.COLLECTION_NAME);
        BsonDocument document = new BsonDocument
        {
            { "PositionName", positionName },
            { "Description", description },
            { "CompanyLink", companyLink },
            { "Qualifications", new BsonArray(qualifications) }
        };

        collection.InsertOne(document);
        return document["_id"].AsObjectId.ToString();
    }

    public static string InsertJobOffer(this IntegrationWebApplicationFactory<Program> factory,
        string positionName, string description, string companyLink, string[] qualifications, 
        string authorName, string authorSurname)
    {
        MongoClient dbClient = new MongoClient(factory.mongoContainer.ConnectionString);
        var database = dbClient.GetDatabase(factory.DATABASE_NAME);
        var collection = database.GetCollection<BsonDocument>(factory.COLLECTION_NAME);
        BsonDocument document = new BsonDocument
        {
            { "PositionName", positionName },
            { "Description", description },
            { "CompanyLink", companyLink },
            { "Qualifications", new BsonArray(qualifications) },
            { "CreatedBy", new BsonDocument
                {
                    { "GlobalId", Guid.NewGuid() },
                    { "Name", authorName},
                    { "Surname", authorSurname}
                }
            }

        };
        collection.InsertOne(document);
        return document["_id"].AsObjectId.ToString();
    }

    public static JobOffer GetJobOfferById(this IntegrationWebApplicationFactory<Program> factory, string id)
    {
        MongoClient dbClient = new MongoClient(factory.mongoContainer.ConnectionString);
        var database = dbClient.GetDatabase(factory.DATABASE_NAME);
        var collection = database.GetCollection<JobOffer>(factory.COLLECTION_NAME);
        return collection.Find(x => x.Id == ObjectId.Parse(id)).FirstOrDefault();
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

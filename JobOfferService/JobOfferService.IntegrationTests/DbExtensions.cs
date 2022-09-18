using System;

using MongoDB.Bson;
using MongoDB.Driver;

using JobOfferService.Model;

namespace JobOfferService.IntegrationTests;

public static class DbExtensions
{
    public static string InsertJobOffer(this IntegrationWebApplicationFactory<Program> factory,
        string positionName, string description, string companyLink, string[] qualifications,
        string authorName, string authorSurname, Guid authorId)
    {
        MongoClient dbClient = new MongoClient(factory.mongoContainer.ConnectionString);
        var database = dbClient.GetDatabase(factory.DATABASE_NAME);
        var collection = database.GetCollection<BsonDocument>(factory.COLLECTION_NAME);

        var profile = factory.InsertProfile(authorName, authorSurname, authorId);

        BsonDocument document = new BsonDocument
        {
            { "PositionName", positionName },
            { "Description", description },
            { "CompanyLink", companyLink },
            { "Qualifications", new BsonArray(qualifications) },
            { "CreatedBy", profile }

        };
        collection.InsertOne(document);

        return document["_id"].AsObjectId.ToString();
    }

    public static BsonDocument InsertProfile(this IntegrationWebApplicationFactory<Program> factory,
        string authorName, string authorSurname, Guid authorId)
    {
        MongoClient dbClient = new MongoClient(factory.mongoContainer.ConnectionString);
        var database = dbClient.GetDatabase(factory.DATABASE_NAME);
        var profieCollection = database.GetCollection<BsonDocument>(factory.PROFILE_COLLECTION_NAME);

        var profileDoc = new BsonDocument
                {
                    { "GlobalId", new BsonBinaryData(authorId, GuidRepresentation.Standard) },
                    { "Name", authorName},
                    { "Surname", authorSurname}
                };

        profieCollection.InsertOne(profileDoc);
        return profileDoc;
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

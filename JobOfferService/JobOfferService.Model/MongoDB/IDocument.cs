using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

using JobOfferService.Model.Serialization;

namespace JobOfferService.Model.MongoDB;
public interface IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [JsonConverter(typeof(ObjectIdConverter))]
    ObjectId Id { get; set; }

    DateTime CreatedAt { get; }
}



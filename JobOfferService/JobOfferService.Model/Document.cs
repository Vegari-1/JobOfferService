using MongoDB.Bson;

namespace JobOfferService.Model
{
	public class Document : IDocument
	{
		public ObjectId Id { get; set; }

		public DateTime CreatedAt => Id.CreationTime;
    }
}


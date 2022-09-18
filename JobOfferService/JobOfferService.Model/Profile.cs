using JobOfferService.Model.MongoDB;

namespace JobOfferService.Model;

[BsonCollection("profile")]
public class Profile : Document
{
    public Guid GlobalId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Avatar { get; set; }

    public IEnumerable<Guid> Blocked { get; set; } = new List<Guid>();
    public IEnumerable<Guid> BlockedBy { get; set; } = new List<Guid>();
}

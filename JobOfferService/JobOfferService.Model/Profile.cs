using JobOfferService.Model.MongoDB;

namespace JobOfferService.Model;

public class Profile : Document
{
    public Guid GlobalId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Avatar { get; set; }
}

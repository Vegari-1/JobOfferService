namespace JobOfferService.Model;

[BsonCollection("jobOffer")]
public class JobOffer : Document
{
    public string PositionName { get; set; }
    public string Description { get; set; }
    //public ISet<string> Qualifications { get; set; }
    //public string Company { get; set; }
}


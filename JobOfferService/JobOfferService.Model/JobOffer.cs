namespace JobOfferService.Model;

[BsonCollection("jobOffer")]
public class JobOffer : Document
{
    public string PositionName { get; set; }
    public string Description { get; set; }
    public IList<string> Qualifications { get; set; }
    public string CompanyLink { get; set; }
    public Profile CreatedBy { get; set; }
}


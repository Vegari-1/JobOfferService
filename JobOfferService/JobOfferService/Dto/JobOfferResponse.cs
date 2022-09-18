namespace JobOfferService.Dto;

public class JobOfferResponse
{
    public string Id { get; set; }
    public string PositionName { get; set; }
    public string Description { get; set; }
    public IList<string> Qualifications { get; set; }
    public string CompanyLink { get; set; }
    public ProfileResponse Profile { get; set; }
}

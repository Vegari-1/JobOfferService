namespace JobOfferService.Dto;

public class JobOfferPostRequest
{
    public string PositionName { get; set; }
    public string Description { get; set; }
    public IList<string> Qualifications { get; set; }
    public string CompanyLink { get; set; }
    public ProfileRequest Profile { get; set; }
}
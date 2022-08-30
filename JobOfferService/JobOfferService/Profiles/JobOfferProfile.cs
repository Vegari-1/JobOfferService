using JobOfferService.Dto;
using JobOfferService.Model;

namespace JobOfferService;

public class JobOfferProfile : AutoMapper.Profile
{
    public JobOfferProfile()
    {
        // Source -> Target
        CreateMap<JobOffer, JobOfferResponse>();
        CreateMap<JobOfferRequest, JobOffer>();
    }
}


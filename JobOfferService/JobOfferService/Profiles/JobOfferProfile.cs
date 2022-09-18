using JobOfferService.Dto;
using JobOfferService.Model;

namespace JobOfferService;

public class JobOfferProfile : AutoMapper.Profile
{
    public JobOfferProfile()
    {
        // Source -> Target
        CreateMap<JobOffer, JobOfferResponse>().ForMember(dest => dest.Profile, src => src.MapFrom(s => s.CreatedBy));
        CreateMap<JobOfferPostRequest, JobOffer>();
        CreateMap<JobOfferPutRequest, JobOffer>();
    }
}


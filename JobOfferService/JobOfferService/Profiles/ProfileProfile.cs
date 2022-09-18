using JobOfferService.Dto;
using JobOfferService.Model;

namespace JobOfferService;

public class ProfileProfile : AutoMapper.Profile
{
    public ProfileProfile()
    {
        // Source -> Target
        CreateMap<Profile, ProfileResponse>();
    }
}
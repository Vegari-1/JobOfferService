using JobOfferService.Dto;
using JobOfferService.Model;

namespace JobOfferService
{
	public class JobOfferProfile : AutoMapper.Profile
	{
		public JobOfferProfile()
		{
			CreateMap<JobOffer, JobOfferResponse>();
		}
	}
}


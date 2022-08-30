using JobOfferService.Model;
using PostService.Repository.Interface.Pagination;

namespace JobOfferService.Service.Interface;

public interface IJobOfferService
{
    Task<JobOffer> Save(JobOffer JobOffer);
    Task<PagedList<JobOffer>> Get(PaginationParams paginationParams);
}


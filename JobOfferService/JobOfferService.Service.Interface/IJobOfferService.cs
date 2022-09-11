using JobOfferService.Model;
using PostService.Repository.Interface.Pagination;

namespace JobOfferService.Service.Interface;

public interface IJobOfferService
{
    Task<JobOffer> Save(JobOffer JobOffer);
    Task<PagedList<JobOffer>> Filter(PaginationParams paginationParams, string? query);
    Task<JobOffer> Get(string id);
    Task<JobOffer> Update(string id, JobOffer jobOffer);
    Task Delete(string id);
}
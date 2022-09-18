using JobOfferService.Model;
using JobOfferService.Repository.Interface.Pagination;

namespace JobOfferService.Service.Interface;

public interface IJobOfferService
{
    Task<JobOffer> Save(Guid createdById, JobOffer JobOffer);
    Task<PagedList<JobOffer>> Filter(Guid userId, PaginationParams paginationParams, string? query);
    Task<JobOffer> Get(Guid userId, string id);
    Task<JobOffer> Update(Guid userId, string id, JobOffer jobOffer);
    Task Delete(Guid userId, string id);
}
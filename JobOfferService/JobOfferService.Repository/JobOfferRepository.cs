using JobOfferService.Repository.Interface;
using JobOfferService.Model;

namespace JobOfferService.Repository;

public class JobOfferRepository : Repository<JobOffer>, IJobOfferRepository
{
    public JobOfferRepository(IMongoDbSettings settings) : base(settings)
    {
    }
}


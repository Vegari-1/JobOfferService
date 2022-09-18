using JobOfferService.Repository.Interface;
using JobOfferService.Model;

namespace JobOfferService.Repository;

public class ProfileRepository : Repository<Profile>, IProfileRepository
{
    public ProfileRepository(IMongoDbSettings settings) : base(settings)
    {
    }
}


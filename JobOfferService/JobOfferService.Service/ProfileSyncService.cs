using Microsoft.Extensions.Logging;

using MongoDB.Driver;

using BusService;
using BusService.Contracts;

using JobOfferService.Model;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;

namespace JobOfferService.Service;
public class ProfileSyncService : ConsumerBase<Profile, ProfileContract>, IProfileSyncService
{
    private readonly IMessageBusService _messageBusService;
    private readonly IJobOfferRepository _jobOfferRepository;

    public ProfileSyncService(IMessageBusService messageBusService, IJobOfferRepository jobOfferRepository, ILogger<ProfileSyncService> logger) : base(logger)
    {
        _messageBusService = messageBusService;
        _jobOfferRepository = jobOfferRepository;
    }

    public override Task PublishAsync(Profile entity, string action)
    {
        // Implement when it is needed
        throw new NotImplementedException();
    }

    public override Task SynchronizeAsync(ProfileContract entity, string action)
    {
        var filter = Builders<JobOffer>.Filter.Eq(x => x.CreatedBy.GlobalId, entity.Id);
        if (action == Events.Updated)
        {
            var update = Builders<JobOffer>.Update
                .Set(x => x.CreatedBy.Name, entity.Name)
                .Set(x => x.CreatedBy.Surname, entity.Surname);
            //.Set(x => x.CreatedBy.Avatar, entity.Avatar);
            return _jobOfferRepository.UpdateManyAsync(filter, update);
        }
        else if (action == Events.Deleted)
        {
            var update = Builders<JobOffer>.Update
                .Set(x => x.CreatedBy.Name, "Removed user")
                .Set(x => x.CreatedBy.Surname, "")
                .Set(x => x.CreatedBy.Avatar, "");
            return _jobOfferRepository.UpdateManyAsync(filter, update);
        }
        return Task.CompletedTask;
    }
}

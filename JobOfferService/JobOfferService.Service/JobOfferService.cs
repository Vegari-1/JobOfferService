
using System.Text;
using Newtonsoft.Json;

using BusService;
using BusService.Routing;

using JobOfferService.Model;
using JobOfferService.Repository.Interface;
using JobOfferService.Service.Interface;

namespace JobOfferService.Service;
public class JobOfferService : IJobOfferService
{

    private readonly IJobOfferRepository _jobOfferRepository;
    private readonly IMessageBusService _messageBusService;

    public JobOfferService(IJobOfferRepository jobOfferRepository, IMessageBusService messageBusService)
    {
        _jobOfferRepository = jobOfferRepository;
        _messageBusService = messageBusService;
    }

    public Task<JobOffer> Create(JobOffer jobOffer)
    {
        throw new NotImplementedException();
    }

    public Task<IList<JobOffer>> Filter()
    {
        return Task.Run<IList<JobOffer>>(() => _jobOfferRepository.AsQueryable().ToList());
    }

    // Example of how to publish message to message queue
    public async Task<JobOffer> PublishToQue(string Id)
    {
        var jobOffer = await _jobOfferRepository.FindByIdAsync(Id);
        var data = JsonConvert.SerializeObject(jobOffer);
        var bdata = Encoding.UTF8.GetBytes(data);
        if (data != null)
        {
            _messageBusService.PublishEvent(SubjectBuilder.Build(Topics.JobOffer, Events.Created), bdata);
        }
        return jobOffer;
    }
}


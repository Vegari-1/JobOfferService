using AutoMapper;
using JobOfferService.Dto;
using JobOfferService.Model;
using JobOfferService.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Prometheus;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobOfferService
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOfferController : Controller
    {
        private readonly IJobOfferService _jobOfferService;
        private readonly IMapper _mapper;
        private readonly ITracer _tracer;

        Counter counter = Metrics.CreateCounter("job_offer_service_counter", "job offer counter");

        public JobOfferController(IJobOfferService jobOfferService, IMapper mapper, ITracer tracer)
        {
            _jobOfferService = jobOfferService;
            _mapper = mapper;
            _tracer = tracer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<JobOffer>), 200)]
        public async Task<IActionResult> Filter()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("filter job offers");

            counter.Inc();

            var jobOfferList = await _jobOfferService.Filter();
            var response = _mapper.Map<ICollection<JobOfferResponse>>(jobOfferList);
            return Ok(response);
        }

        // Just for example purposes
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ICollection<JobOffer>), 200)]
        public async Task<IActionResult> Publish(JobOfferResponse JobOfferResponse)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("public job offer");

            counter.Inc();

            var jobOffer = await _jobOfferService.PublishToQue(JobOfferResponse.Id);
            return Ok(_mapper.Map<JobOfferResponse>(jobOffer));
        }
    }
}


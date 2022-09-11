using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Prometheus;

using JobOfferService.Dto;
using JobOfferService.Model;
using JobOfferService.Service.Interface;
using PostService.Repository.Interface.Pagination;

namespace JobOfferService;

[Route("api/[controller]")]
[ApiController]
public class JobOfferController : Controller
{
    private readonly IJobOfferService _jobOfferService;
    private readonly IMapper _mapper;
    private readonly ITracer _tracer;

    readonly Counter counter = Metrics.CreateCounter("job_offer_service_counter", "job offer counter");

    public JobOfferController(IJobOfferService jobOfferService, IMapper mapper, ITracer tracer)
    {
        _jobOfferService = jobOfferService;
        _mapper = mapper;
        _tracer = tracer;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParams paginationParams, string? query)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("get job offers");

        counter.Inc();

        var jobOfferList = await _jobOfferService.Filter(paginationParams, query);
        return Ok(jobOfferList.ToPagedList(_mapper.Map<List<JobOfferResponse>>(jobOfferList.Items)));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("get job offer by id");

        counter.Inc();

        var jobOffer = await _jobOfferService.Get(id);

        if (jobOffer == null)
            return NotFound();

        return Ok(_mapper.Map<JobOfferResponse>(jobOffer));
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] JobOfferPostRequest request)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("save job offer");

        counter.Inc();

        var jobOffer = await _jobOfferService.Save(_mapper.Map<JobOffer>(request));
        return StatusCode(StatusCodes.Status201Created, _mapper.Map<JobOfferResponse>(jobOffer));
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] JobOfferPutRequest request)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("update job offer");

        counter.Inc();

        var jobOffer = await _jobOfferService.Update(id, _mapper.Map<JobOffer>(request));

        if (jobOffer == null)
            return NotFound();

        return Ok(_mapper.Map<JobOfferResponse>(jobOffer));
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("delete job offer");

        counter.Inc();

        await _jobOfferService.Delete(id);
        return Ok();
    }
}


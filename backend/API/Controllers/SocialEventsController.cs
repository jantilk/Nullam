using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/social-events")]
public class SocialEventsController : NullamControllerBase
{
    private readonly ISocialEventsService _socialEventsService;

    public SocialEventsController(ISocialEventsService socialEventsService)
    {
        _socialEventsService = socialEventsService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddSocialEventRequest request)
    {
        return HandleOperationResult(await _socialEventsService.Add(request));
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SortingOption? orderBy, [FromQuery] FilterDto? filter)
    {
        return HandleOperationResult(await _socialEventsService.Get(orderBy, filter));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return HandleOperationResult(await _socialEventsService.GetById(id));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateSocialEventRequest request)
    {
        return HandleOperationResult(await _socialEventsService.Update(id, request));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return HandleOperationResult(await _socialEventsService.Delete(id));
    }
}
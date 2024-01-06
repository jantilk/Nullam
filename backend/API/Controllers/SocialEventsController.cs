using Application.DTOs;
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
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SortingOption? orderBy, [FromQuery] FilterDto? filter)
    {
        return HandleOperationResult(await _socialEventsService.Get(orderBy, filter));
    }
}
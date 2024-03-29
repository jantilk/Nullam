using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/social-events/{socialEventId:guid}/participating-persons")]
public class SocialEventPersonsController : NullamControllerBase
{
    private readonly ISocialEventPersonsService _socialEventPersonsService;

    public SocialEventPersonsController(ISocialEventPersonsService socialEventPersonsService)
    {
        _socialEventPersonsService = socialEventPersonsService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromRoute] Guid socialEventId, AddSocialEventPersonRequest request)
    {
        return HandleOperationResult(await _socialEventPersonsService.Add(socialEventId, request));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetSocialEventPersonsBySocialEventId([FromRoute] Guid socialEventId)
    {
        return HandleOperationResult(await _socialEventPersonsService.GetSocialEventPersonsBySocialEventId(socialEventId));
    }
    
    [HttpGet("{personId:guid}")]
    public async Task<IActionResult> GetSocialEventsByPersonId([FromRoute] Guid socialEventId, [FromRoute] Guid personId)
    {
        return HandleOperationResult(await _socialEventPersonsService.GetSocialEventsByPersonId(socialEventId, personId));
    }
    
    [HttpPut("{personId:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid socialEventId, [FromRoute] Guid personId, UpdateSocialEventPersonRequest request)
    {
        return HandleOperationResult(await _socialEventPersonsService.Update(socialEventId, personId, request));
    }

    [HttpDelete("{personId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid socialEventId, [FromRoute] Guid personId)
    {
        return HandleOperationResult(await _socialEventPersonsService.Delete(socialEventId, personId));
    }
}
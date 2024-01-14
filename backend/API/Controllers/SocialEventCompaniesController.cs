using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/social-events/{socialEventId:guid}/participating-companies")]
public class SocialEventCompaniesController : NullamControllerBase
{
    private readonly ISocialEventCompaniesService _socialEventCompaniesService;

    public SocialEventCompaniesController(ISocialEventCompaniesService socialEventCompaniesService)
    {
        _socialEventCompaniesService = socialEventCompaniesService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromRoute] Guid socialEventId, AddSocialEventCompanyRequest request)
    {
        return HandleOperationResult(await _socialEventCompaniesService.Add(socialEventId, request));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetSocialEventCompaniesBySocialEventId([FromRoute] Guid socialEventId)
    {
        return HandleOperationResult(await _socialEventCompaniesService.GetBySocialEventId(socialEventId));
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetSocialEventsByCompanyId([FromRoute] Guid socialEventId, [FromRoute] Guid companyId)
    {
        return HandleOperationResult(await _socialEventCompaniesService.GetSocialEventsByCompanyId(socialEventId, companyId));
    }

    [HttpPut("{companyId:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid socialEventId, [FromRoute] Guid companyId, UpdateSocialEventCompanyRequest request)
    {
        return HandleOperationResult(await _socialEventCompaniesService.Update(socialEventId, companyId, request));
    }
    
    [HttpDelete("{companyId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid socialEventId, [FromRoute] Guid companyId)
    {
        return HandleOperationResult(await _socialEventCompaniesService.Delete(socialEventId, companyId));
    }
}
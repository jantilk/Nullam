using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/social-events/{socialEventId:guid}/participants/companies")]
public class SocialEventCompaniesController : NullamControllerBase
{
    private readonly ISocialEventCompaniesService _socialEventCompaniesService;

    public SocialEventCompaniesController(ISocialEventCompaniesService socialEventCompaniesService)
    {
        _socialEventCompaniesService = socialEventCompaniesService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCompaniesBySocialEventId([FromRoute] Guid socialEventId)
    {
        return HandleOperationResult(await _socialEventCompaniesService.GetCompaniesBySocialEventId(socialEventId));
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromRoute] Guid socialEventId, AddSocialEventCompanyRequest request)
    {
        return HandleOperationResult(await _socialEventCompaniesService.Add(socialEventId, request));
    }

    [HttpPut("{companyId:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid socialEventId, [FromRoute] Guid companyId, UpdateSocialEventCompanyRequest request)
    {
        return HandleOperationResult(await _socialEventCompaniesService.Update(socialEventId, companyId, request));
    }
}
using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/resources")]
public class ResourcesController : NullamControllerBase
{
    private readonly IResourceService _resourceService;

    public ResourcesController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddResourceRequest request)
    {
        return HandleOperationResult(await _resourceService.Add(request));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByType([FromQuery] string type)
    {
        return HandleOperationResult(await _resourceService.GetByType(type));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateResourceRequest request)
    {
        return HandleOperationResult(await _resourceService.Update(id, request));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return HandleOperationResult(await _resourceService.Delete(id));
    }
}
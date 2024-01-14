using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/companies")]
public class CompaniesController : NullamControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FilterDto? filter)
    {
        return HandleOperationResult(await _companyService.Get(filter));
    }
}
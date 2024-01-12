using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/persons")]
public class PersonsController : NullamControllerBase
{
    private readonly IPersonService _personService;

    public PersonsController(IPersonService personService)
    {
        _personService = personService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FilterDto? filter)
    {
        return HandleOperationResult(await _personService.Get(filter));
    }
}
using Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class NullamControllerBase : ControllerBase
{
    protected IActionResult HandleOperationResult<T>(OperationResult<T>? result)
    {
        if (result == null) {
            return NotFound(new { error = "Resource not found", success = false });
        }
        
        if (!result.IsSuccess) {
            return BadRequest(new { error = result.Error, success = false });
        }
        
        if (typeof(T) == typeof(bool))
        {
            return Ok(new { success = true });
        }
        
        return Ok(new { success = true, data = result.Value });
    }
}
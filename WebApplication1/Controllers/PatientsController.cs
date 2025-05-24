using Microsoft.AspNetCore.Mvc;
using WebApplication1.Exceptions;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController(IDbService dbService) : ControllerBase
{
    [HttpGet("{IdPatient}")]
    public async Task<IActionResult> GetPatient([FromRoute] int IdPatient)
    {
        try
        {
            return Ok(await dbService.GetPatient(IdPatient));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
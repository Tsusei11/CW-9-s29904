using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class PrescriptionsController(IDbService dbService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionCreateDto prescription)
    {
        try
        {
            var result = await dbService.CreatePrescription(prescription);
            return Created($"prescriptions/{result}", result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (IncorrectDateException e)
        {
            return BadRequest(e.Message);
        }
        catch (NoMedicamentsProvidedException e)
        {
            return BadRequest(e.Message);
        }
    }
}
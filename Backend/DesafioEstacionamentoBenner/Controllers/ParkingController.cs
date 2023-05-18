using DesafioEstacionamentoBenner.DTO_s;
using DesafioEstacionamentoBenner.Services.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DesafioEstacionamentoBenner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingController : ControllerBase
{
    private readonly IParkingService _service;

    public ParkingController(IParkingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Parking>>> GetAsync(long id = 0)
    {
        if (id == 0)
        {
            return Ok(await _service.GetAllAsync());
        }
        else
        {
            return Ok(await _service.GetByIdAsync(id));
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(long id)
    {
        return Ok(await _service.DeleteAsync(id));
    }

    [HttpPost("RegisterEntry")]
    public async Task<ActionResult> RegisterEntry(RegisterEntryRequestDTO entity)
    {
        return Ok(await _service.RegisterEntry(entity));
    }

    [HttpPut("RegisterDeparture")]
    public async Task<ActionResult> RegisterDeparture(RegisterDepartureRequestDTO entity)
    {
        return Ok(await _service.RegisterDeparture(entity));
    }
}

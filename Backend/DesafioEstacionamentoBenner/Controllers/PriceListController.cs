using DesafioEstacionamentoBenner.Services.Interfaces;
using Infrastructure.DTO;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DesafioEstacionamentoBenner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriceListController : ControllerBase
{
    private readonly IPriceListService _service;

    public PriceListController(IPriceListService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<PriceList>>> GetAsync()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<List<PriceList>>> GetByIdAsync(long id = 0)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(PriceList entity)
    {
        if (entity.FinalDate < entity.InitialDate)
        {
            throw new AppException("A data de saída precisa ser maior que a data de entrada!");
        }
        return Ok(await _service.PostAsync(entity));
    }

    [HttpPut]
    public async Task<ActionResult> PutAsync(PriceList entity)
    {
        return Ok(await _service.PutAsync(entity));
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(long id)
    {
        return Ok(await _service.DeleteAsync(id));
    }
}

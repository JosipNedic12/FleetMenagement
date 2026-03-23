using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _service;

    public VehiclesController(IVehicleService service) => _service = service;

    [HttpGet]
    [Authorize(Roles = "Admin,FleetManager,ReadOnly")]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,FleetManager,ReadOnly")]
    public async Task<ActionResult<VehicleDto>> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "Admin,FleetManager")]
    public async Task<ActionResult<VehicleDto>> Create(CreateVehicleDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.VehicleId }, result);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin,FleetManager")]
    public async Task<ActionResult<VehicleDto>> Update(int id, UpdateVehicleDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}

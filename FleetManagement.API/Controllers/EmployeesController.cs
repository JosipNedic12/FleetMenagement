using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service) => _service = service;

    [HttpGet]
    [Authorize(Roles = "Admin,FleetManager,ReadOnly")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,FleetManager,ReadOnly")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "Admin,FleetManager")]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.EmployeeId }, result);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin,FleetManager")]
    public async Task<ActionResult<EmployeeDto>> Update(int id, UpdateEmployeeDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}

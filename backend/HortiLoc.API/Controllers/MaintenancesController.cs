using HortiLoc.Core.DTOs;
using HortiLoc.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace HortiLoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenancesController : ControllerBase
{
    private readonly MaintenanceService _maintenanceService;

    public MaintenancesController(
        MaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var maintenances =
            await _maintenanceService.GetAllAsync();

        return Ok(maintenances);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var maintenance =
            await _maintenanceService.GetByIdAsync(id);

        if (maintenance is null)
            return NotFound();

        return Ok(maintenance);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMaintenanceDto dto)
    {
        try
        {
            var maintenance =
                await _maintenanceService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = maintenance.Id },
                maintenance
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPatch("{id:int}/statut")]
    public async Task<IActionResult> UpdateStatut(
        int id,
        UpdateMaintenanceStatutDto dto)
    {
        try
        {
            var modifie =
                await _maintenanceService.UpdateStatutAsync(id, dto);

            if (!modifie)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
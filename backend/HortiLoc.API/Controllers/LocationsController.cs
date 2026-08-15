using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortiLoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly LocationService _locationService;

    public LocationsController(LocationService locationService)
    {
        _locationService = locationService;
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetAll()
    {
        var locations = await _locationService.GetAllAsync();
        return Ok(locations);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Location>> GetById(int id)
    {
        var location = await _locationService.GetByIdAsync(id);

        if (location is null)
            return NotFound();

        return Ok(location);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<ActionResult<Location>> Create(CreateLocationDto dto)
    {
        try
        {
            var location = await _locationService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = location.Id },
                location
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

    [Authorize(Roles = "ADMIN")]
    [HttpPatch("{id:int}/retour")]
    public async Task<IActionResult> Return(int id)
    {
        try
        {
            var retourne = await _locationService.ReturnAsync(id);

            if (!retourne)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [Authorize(Roles = "CLIENT")]
    [HttpGet("mes-locations")]
    public async Task<IActionResult> GetMesLocations()
    {
        var clientIdClaim = User.FindFirst("clientId")?.Value;

        if (!int.TryParse(clientIdClaim, out var clientId))
        {
            return Unauthorized(
                "Le compte connecté n'est associé à aucun client."
            );
        }

        var locations =
            await _locationService.GetByClientIdAsync(clientId);

        return Ok(locations);
    }
}
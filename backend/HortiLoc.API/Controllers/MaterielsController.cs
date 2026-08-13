using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace HortiLoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterielsController : ControllerBase
{
    private readonly MaterielService _materielService;

    public MaterielsController(MaterielService materielService)
    {
        _materielService = materielService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Materiel>>> GetAll()
    {
        var materiels = await _materielService.GetAllAsync();
        return Ok(materiels);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Materiel>> GetById(int id)
    {
        var materiel = await _materielService.GetByIdAsync(id);

        if (materiel is null)
            return NotFound();

        return Ok(materiel);
    }

    [HttpPost]
    public async Task<ActionResult<Materiel>> Create(CreateMaterielDto dto)
    {
        try
        {
            var materiel = await _materielService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = materiel.Id },
                materiel
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateMaterielDto dto)
    {
        try
        {
            bool modifie = await _materielService.UpdateAsync(id, dto);

            if (!modifie)
                return NotFound();

            return NoContent();
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Disable(int id)
    {
        bool desactive = await _materielService.DisableAsync(id);

        if (!desactive)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id:int}/reactiver")]
    public async Task<IActionResult> Reactivate(int id)
    {
        bool reactive = await _materielService.ReactivateAsync(id);

        if (!reactive)
            return NotFound();

        return NoContent();
    }
}
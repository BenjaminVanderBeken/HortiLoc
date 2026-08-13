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
}
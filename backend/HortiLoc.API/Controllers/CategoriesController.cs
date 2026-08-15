using HortiLoc.Core.DTOs;
using HortiLoc.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HortiLoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN")]
public class CategoriesController : ControllerBase
{
    private readonly CategorieService _categorieService;

    public CategoriesController(CategorieService categorieService)
    {
        _categorieService = categorieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _categorieService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categorie = await _categorieService.GetByIdAsync(id);

        return categorie is null
            ? NotFound()
            : Ok(categorie);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategorieDto dto)
    {
        try
        {
            var categorie = await _categorieService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = categorie.Id },
                categorie
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCategorieDto dto)
    {
        try
        {
            var categorie =
                await _categorieService.UpdateAsync(id, dto);

            return categorie is null
                ? NotFound()
                : Ok(categorie);
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
        var resultat = await _categorieService.DisableAsync(id);

        return resultat
            ? NoContent()
            : NotFound();
    }

    [HttpPatch("{id:int}/reactiver")]
    public async Task<IActionResult> Reactivate(int id)
    {
        var resultat = await _categorieService.ReactivateAsync(id);

        return resultat
            ? NoContent()
            : NotFound();
    }
}
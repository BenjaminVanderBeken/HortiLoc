using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HortiLoc.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN")]
public class ClientsController : ControllerBase
{
    private readonly ClientService _clientService;

    public ClientsController(ClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetAll()
    {
        var clients = await _clientService.GetAllAsync();
        return Ok(clients);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await _clientService.GetByIdAsync(id);

        if (client is null)
            return NotFound();

        return Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<Client>> Create(CreateClientDto dto)
    {
        try
        {
            var client = await _clientService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = client.Id },
                client
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
    public async Task<IActionResult> Update(int id, UpdateClientDto dto)
    {
        try
        {
            bool modifie = await _clientService.UpdateAsync(id, dto);

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
        bool desactive = await _clientService.DisableAsync(id);

        if (!desactive)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id:int}/reactiver")]
    public async Task<IActionResult> Reactivate(int id)
    {
        bool reactive = await _clientService.ReactivateAsync(id);

        if (!reactive)
            return NotFound();

        return NoContent();
    }
}
using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;

namespace HortiLoc.Core.Services;

public class ClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _clientRepository.GetByIdAsync(id);
    }

    public async Task<Client> CreateAsync(CreateClientDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nom))
            throw new ArgumentException("Le nom est obligatoire.");

        if (string.IsNullOrWhiteSpace(dto.Prenom))
            throw new ArgumentException("Le prénom est obligatoire.");

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            bool existe = await _clientRepository.ExistsByEmailAsync(dto.Email);

            if (existe)
                throw new InvalidOperationException("Cette adresse e-mail est déjà utilisée.");
        }

        var client = new Client
        {
            Nom = dto.Nom.Trim(),
            Prenom = dto.Prenom.Trim(),
            Email = dto.Email?.Trim(),
            Telephone = dto.Telephone?.Trim(),
            Adresse = dto.Adresse?.Trim(),
            Actif = true
        };

        client.Id = await _clientRepository.CreateAsync(client);

        return client;
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientDto dto)
    {
        var client = await _clientRepository.GetByIdAsync(id);

        if (client is null)
            return false;

        if (string.IsNullOrWhiteSpace(dto.Nom))
            throw new ArgumentException("Le nom est obligatoire.");

        if (string.IsNullOrWhiteSpace(dto.Prenom))
            throw new ArgumentException("Le prénom est obligatoire.");

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            bool existe = await _clientRepository.ExistsByEmailAsync(dto.Email, id);

            if (existe)
                throw new InvalidOperationException("Cette adresse e-mail est déjà utilisée.");
        }

        client.Nom = dto.Nom.Trim();
        client.Prenom = dto.Prenom.Trim();
        client.Email = dto.Email?.Trim();
        client.Telephone = dto.Telephone?.Trim();
        client.Adresse = dto.Adresse?.Trim();

        return await _clientRepository.UpdateAsync(client);
    }

    public async Task<bool> DisableAsync(int id)
    {
        return await _clientRepository.DisableAsync(id);
    }

    public async Task<bool> ReactivateAsync(int id)
    {
        return await _clientRepository.ReactivateAsync(id);
    }
}
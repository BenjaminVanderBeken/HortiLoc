using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;

namespace HortiLoc.Core.Services;

public class LocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IMaterielRepository _materielRepository;

    public LocationService(
        ILocationRepository locationRepository,
        IClientRepository clientRepository,
        IMaterielRepository materielRepository)
    {
        _locationRepository = locationRepository;
        _clientRepository = clientRepository;
        _materielRepository = materielRepository;
    }

    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        return await _locationRepository.GetAllAsync();
    }
public Task<IEnumerable<Location>> GetByClientIdAsync(int clientId)
{
    if (clientId <= 0)
        throw new ArgumentException("Identifiant client invalide.");

    return _locationRepository.GetByClientIdAsync(clientId);
}
    public async Task<Location?> GetByIdAsync(int id)
    {
        return await _locationRepository.GetByIdAsync(id);
    }

    public async Task<Location> CreateAsync(CreateLocationDto dto)
    {
        if (dto.ClientId <= 0)
            throw new ArgumentException("Le client est obligatoire.");

        if (dto.DateFinPrevue.Date < dto.DateDebut.Date)
            throw new ArgumentException(
                "La date de fin ne peut pas être antérieure à la date de début."
            );

        if (dto.Details.Count == 0)
            throw new ArgumentException(
                "Une location doit contenir au moins un matériel."
            );

        var client = await _clientRepository.GetByIdAsync(dto.ClientId);

        if (client is null)
            throw new InvalidOperationException("Le client n'existe pas.");

        if (!client.Actif)
            throw new InvalidOperationException(
                "Un client inactif ne peut pas effectuer une location."
            );

        int nombreJours = (dto.DateFinPrevue.Date - dto.DateDebut.Date).Days + 1;

        var location = new Location
        {
            ClientId = dto.ClientId,
            DateDebut = dto.DateDebut.Date,
            DateFinPrevue = dto.DateFinPrevue.Date,
            Statut = "EN_COURS",
            Notes = dto.Notes?.Trim()
        };

        foreach (var ligne in dto.Details)
        {
            if (ligne.Quantite <= 0)
                throw new ArgumentException(
                    "La quantité louée doit être supérieure à zéro."
                );

            var materiel = await _materielRepository.GetByIdAsync(ligne.MaterielId);

            if (materiel is null)
                throw new InvalidOperationException(
                    $"Le matériel {ligne.MaterielId} n'existe pas."
                );

            if (!materiel.Actif)
                throw new InvalidOperationException(
                    $"{materiel.Nom} est inactif."
                );

            if (ligne.Quantite > materiel.QuantiteDisponible)
                throw new InvalidOperationException(
                    $"Stock insuffisant pour {materiel.Nom}."
                );

            decimal sousTotal =
                materiel.PrixJournalier * ligne.Quantite * nombreJours;

            location.Details.Add(new DetailLocation
            {
                MaterielId = materiel.Id,
                MaterielNom = materiel.Nom,
                Quantite = ligne.Quantite,
                PrixJournalier = materiel.PrixJournalier,
                SousTotal = sousTotal
            });
        }

        location.MontantTotal =
            location.Details.Sum(detail => detail.SousTotal);

        location.Id = await _locationRepository.CreateAsync(location);

var locationCreee = await _locationRepository.GetByIdAsync(location.Id);

return locationCreee
    ?? throw new InvalidOperationException(
        "Impossible de récupérer la location après sa création."
    );
    }

    public async Task<bool> ReturnAsync(int id)
    {
        var location = await _locationRepository.GetByIdAsync(id);

        if (location is null)
            return false;

        if (location.Statut == "RETOURNEE")
            throw new InvalidOperationException(
                "Cette location a déjà été retournée."
            );

        if (location.Statut == "ANNULEE")
            throw new InvalidOperationException(
                "Une location annulée ne peut pas être retournée."
            );
            if (DateTime.Today < location.DateDebut.Date)
    throw new InvalidOperationException(
        "Une location ne peut pas être retournée avant sa date de début."
    );

        return await _locationRepository.ReturnAsync(id, DateTime.Today);
    }
}
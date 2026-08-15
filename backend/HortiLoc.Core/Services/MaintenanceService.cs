using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;

namespace HortiLoc.Core.Services;

public class MaintenanceService
{
    private readonly IMaintenanceRepository _maintenanceRepository;
    private readonly IMaterielRepository _materielRepository;

    public MaintenanceService(
        IMaintenanceRepository maintenanceRepository,
        IMaterielRepository materielRepository)
    {
        _maintenanceRepository = maintenanceRepository;
        _materielRepository = materielRepository;
    }

    public Task<IEnumerable<Maintenance>> GetAllAsync()
    {
        return _maintenanceRepository.GetAllAsync();
    }

    public Task<Maintenance?> GetByIdAsync(int id)
    {
        return _maintenanceRepository.GetByIdAsync(id);
    }

    public async Task<Maintenance> CreateAsync(CreateMaintenanceDto dto)
    {
        if (dto.MaterielId <= 0)
            throw new ArgumentException("Le matériel est obligatoire.");

        if (string.IsNullOrWhiteSpace(dto.Motif))
            throw new ArgumentException("Le motif est obligatoire.");

        var materiel = await _materielRepository.GetByIdAsync(dto.MaterielId);

        if (materiel is null)
            throw new InvalidOperationException(
                "Le matériel n'existe pas."
            );

        if (!materiel.Actif)
            throw new InvalidOperationException(
                "Une maintenance ne peut pas être créée pour un matériel désactivé."
            );

        var maintenance = new Maintenance
        {
            MaterielId = dto.MaterielId,
            DateDebut = dto.DateDebut,
            Motif = dto.Motif.Trim(),
            Statut = "PLANIFIEE"
        };

        maintenance.Id =
            await _maintenanceRepository.CreateAsync(maintenance);

        var maintenanceCreee =
            await _maintenanceRepository.GetByIdAsync(maintenance.Id);

        return maintenanceCreee
            ?? throw new InvalidOperationException(
                "Impossible de récupérer la maintenance après sa création."
            );
    }

    public async Task<Maintenance?> UpdateAsync(
        int id,
        UpdateMaintenanceDto dto)
    {
        var maintenance =
            await _maintenanceRepository.GetByIdAsync(id);

        if (maintenance is null)
            return null;

        if (maintenance.Statut == "TERMINEE")
            throw new InvalidOperationException(
                "Une maintenance terminée ne peut plus être modifiée."
            );

        if (dto.MaterielId <= 0)
            throw new ArgumentException(
                "Le matériel est obligatoire."
            );

        if (string.IsNullOrWhiteSpace(dto.Motif))
            throw new ArgumentException(
                "Le motif est obligatoire."
            );

        var materiel =
            await _materielRepository.GetByIdAsync(dto.MaterielId);

        if (materiel is null)
            throw new InvalidOperationException(
                "Le matériel n'existe pas."
            );

        if (!materiel.Actif)
            throw new InvalidOperationException(
                "Le matériel sélectionné est désactivé."
            );

        maintenance.MaterielId = dto.MaterielId;
        maintenance.DateDebut = dto.DateDebut;
        maintenance.Motif = dto.Motif.Trim();

        await _maintenanceRepository.UpdateAsync(maintenance);

        return await _maintenanceRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateStatutAsync(
        int id,
        UpdateMaintenanceStatutDto dto)
    {
        var maintenance =
            await _maintenanceRepository.GetByIdAsync(id);

        if (maintenance is null)
            return false;

        var statut = dto.Statut.Trim().ToUpperInvariant();

        if (statut != "PLANIFIEE"
            && statut != "EN_COURS"
            && statut != "TERMINEE")
        {
            throw new ArgumentException(
                "Le statut doit être PLANIFIEE, EN_COURS ou TERMINEE."
            );
        }

        if (statut == "EN_COURS"
            && DateTime.Today < maintenance.DateDebut.Date)
        {
            throw new InvalidOperationException(
                "Une maintenance ne peut pas commencer avant sa date de début."
            );
        }

        DateTime? dateFin =
            statut == "TERMINEE"
                ? DateTime.Today
                : null;

        return await _maintenanceRepository.UpdateStatutAsync(
            id,
            statut,
            dateFin
        );
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var maintenance =
            await _maintenanceRepository.GetByIdAsync(id);

        if (maintenance is null)
            return false;

        if (maintenance.Statut != "PLANIFIEE")
            throw new InvalidOperationException(
                "Seule une maintenance planifiée peut être supprimée."
            );

        return await _maintenanceRepository.DeleteAsync(id);
    }
}
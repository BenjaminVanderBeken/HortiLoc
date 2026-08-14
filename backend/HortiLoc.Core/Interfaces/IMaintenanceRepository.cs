using HortiLoc.Core.Entities;

namespace HortiLoc.Core.Interfaces;

public interface IMaintenanceRepository
{
    Task<IEnumerable<Maintenance>> GetAllAsync();
    Task<Maintenance?> GetByIdAsync(int id);
    Task<int> CreateAsync(Maintenance maintenance);
    Task<bool> UpdateStatutAsync(
        int id,
        string statut,
        DateTime? dateFin
    );
}
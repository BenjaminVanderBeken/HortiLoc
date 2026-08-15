using HortiLoc.Core.Entities;

namespace HortiLoc.Core.Interfaces;

public interface IMaintenanceRepository
{
    Task<IEnumerable<Maintenance>> GetAllAsync();
    Task<Maintenance?> GetByIdAsync(int id);
    Task<int> CreateAsync(Maintenance maintenance);
    Task<bool> UpdateAsync(Maintenance maintenance);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStatutAsync(
        int id,
        string statut,
        DateTime? dateFin
    );
}
using HortiLoc.Core.Entities;

namespace HortiLoc.Core.Interfaces;

public interface IMaterielRepository
{
    Task<IEnumerable<Materiel>> GetAllAsync();
    Task<Materiel?> GetByIdAsync(int id);
    Task<int> CreateAsync(Materiel materiel);
    Task<bool> UpdateAsync(Materiel materiel);
    Task<bool> DisableAsync(int id);
    Task<bool> ReactivateAsync(int id);
}
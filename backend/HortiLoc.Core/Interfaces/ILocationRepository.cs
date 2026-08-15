using HortiLoc.Core.Entities;

namespace HortiLoc.Core.Interfaces;

public interface ILocationRepository
{
    Task<IEnumerable<Location>> GetAllAsync();
    Task<Location?> GetByIdAsync(int id);
    Task<int> CreateAsync(Location location);
    Task<bool> ReturnAsync(int id, DateTime dateRetour);
    Task<IEnumerable<Location>> GetByClientIdAsync(int clientId);
}
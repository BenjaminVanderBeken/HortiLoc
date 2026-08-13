using HortiLoc.Core.Entities;

namespace HortiLoc.Core.Interfaces;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(int id);
    Task<int> CreateAsync(Client client);
    Task<bool> UpdateAsync(Client client);
    Task<bool> DisableAsync(int id);
    Task<bool> ReactivateAsync(int id);
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
}
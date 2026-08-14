using HortiLoc.Core.Entities;

namespace HortiLoc.Core.Interfaces;

public interface ICategorieRepository
{
    Task<IEnumerable<Categorie>> GetAllAsync();
    Task<Categorie?> GetByIdAsync(int id);
    Task<int> CreateAsync(Categorie categorie);
    Task<bool> UpdateAsync(Categorie categorie);
    Task<bool> DisableAsync(int id);
    Task<bool> ReactivateAsync(int id);
    Task<bool> ExistsByNomAsync(string nom, int? excludeId = null);
}
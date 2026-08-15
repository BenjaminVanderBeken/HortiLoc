using HortiLoc.Core.Entities;

namespace HortiLoc.Core.Interfaces;

public interface IUtilisateurRepository
{
    Task<Utilisateur?> GetByEmailAsync(string email);
    Task<int> CreateAsync(Utilisateur utilisateur);
}
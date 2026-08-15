using HortiLoc.Core.Entities;

namespace HortiLoc.Core.Interfaces;

public interface IPasswordService
{
    string HashPassword(
        Utilisateur utilisateur,
        string motDePasse
    );

    bool VerifyPassword(
        Utilisateur utilisateur,
        string motDePasseHash,
        string motDePasse
    );
}
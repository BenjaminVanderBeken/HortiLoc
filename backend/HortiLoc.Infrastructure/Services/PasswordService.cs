using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HortiLoc.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<Utilisateur> _passwordHasher = new();

    public string HashPassword(
        Utilisateur utilisateur,
        string motDePasse)
    {
        return _passwordHasher.HashPassword(
            utilisateur,
            motDePasse
        );
    }

    public bool VerifyPassword(
        Utilisateur utilisateur,
        string motDePasseHash,
        string motDePasse)
    {
        var resultat =
            _passwordHasher.VerifyHashedPassword(
                utilisateur,
                motDePasseHash,
                motDePasse
            );

        return resultat != PasswordVerificationResult.Failed;
    }
}
using HortiLoc.Core.DTOs;
using HortiLoc.Core.Interfaces;

namespace HortiLoc.Core.Services;

public class AuthService
{
    private readonly IUtilisateurRepository _utilisateurRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUtilisateurRepository utilisateurRepository,
        IPasswordService passwordService,
        ITokenService tokenService)
    {
        _utilisateurRepository = utilisateurRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email)
            || string.IsNullOrWhiteSpace(dto.MotDePasse))
        {
            throw new ArgumentException(
                "L'adresse e-mail et le mot de passe sont obligatoires."
            );
        }

        var utilisateur =
            await _utilisateurRepository.GetByEmailAsync(
                dto.Email.Trim()
            );

        if (utilisateur is null)
            throw new UnauthorizedAccessException(
                "Adresse e-mail ou mot de passe incorrect."
            );

        if (!utilisateur.Actif)
            throw new UnauthorizedAccessException(
                "Ce compte est désactivé."
            );

        var motDePasseValide =
            _passwordService.VerifyPassword(
                utilisateur,
                utilisateur.MotDePasseHash,
                dto.MotDePasse
            );

        if (!motDePasseValide)
            throw new UnauthorizedAccessException(
                "Adresse e-mail ou mot de passe incorrect."
            );

        return _tokenService.CreateToken(utilisateur);
    }
}
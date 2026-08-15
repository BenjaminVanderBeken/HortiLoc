using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace HortiLoc.API.Services;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthResultDto CreateToken(Utilisateur utilisateur)
    {
        var key =
            _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "La clé JWT n'est pas configurée."
            );

        var issuer =
            _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "L'émetteur JWT n'est pas configuré."
            );

        var audience =
            _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "L'audience JWT n'est pas configurée."
            );

        var expiration =
            DateTime.UtcNow.AddHours(8);

        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                utilisateur.Id.ToString()
            ),
            new(
                ClaimTypes.Email,
                utilisateur.Email
            ),
            new(
                ClaimTypes.Role,
                utilisateur.Role
            )
        };

        if (utilisateur.ClientId.HasValue)
        {
            claims.Add(
                new Claim(
                    "clientId",
                    utilisateur.ClientId.Value.ToString()
                )
            );
        }

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            );

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

        var token =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

        return new AuthResultDto
        {
            Token =
                new JwtSecurityTokenHandler()
                    .WriteToken(token),
            Email = utilisateur.Email,
            Role = utilisateur.Role,
            ClientId = utilisateur.ClientId,
            Expiration = expiration
        };
    }
}
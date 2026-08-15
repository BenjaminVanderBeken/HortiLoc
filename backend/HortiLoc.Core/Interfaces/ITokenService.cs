using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;

namespace HortiLoc.Core.Interfaces;

public interface ITokenService
{
    AuthResultDto CreateToken(Utilisateur utilisateur);
}
namespace HortiLoc.Core.DTOs;

public class AuthResultDto
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? ClientId { get; set; }
    public DateTime Expiration { get; set; }
}
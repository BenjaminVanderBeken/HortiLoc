namespace HortiLoc.Core.DTOs;

public class UpdateClientDto
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Adresse { get; set; }
}
namespace HortiLoc.Core.Entities;

public class Utilisateur
{
    public int Id { get; set; }
    public int? ClientId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string MotDePasseHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool Actif { get; set; }
    public DateTime DateCreation { get; set; }
}
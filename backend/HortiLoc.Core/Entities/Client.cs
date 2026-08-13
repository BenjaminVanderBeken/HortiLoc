namespace HortiLoc.Core.Entities;

public class Client
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Adresse { get; set; }
    public bool Actif { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime DateModification { get; set; }
}
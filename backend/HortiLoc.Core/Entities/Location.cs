namespace HortiLoc.Core.Entities;

public class Location
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string ClientNom { get; set; } = string.Empty;
    public string ClientPrenom { get; set; } = string.Empty;
    public DateTime DateDebut { get; set; }
    public DateTime DateFinPrevue { get; set; }
    public DateTime? DateRetour { get; set; }
    public string Statut { get; set; } = "EN_ATTENTE";
    public decimal MontantTotal { get; set; }
    public string? Notes { get; set; }
    public DateTime DateCreation { get; set; }
    public List<DetailLocation> Details { get; set; } = [];
}
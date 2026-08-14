namespace HortiLoc.Core.Entities;

public class Maintenance
{
    public int Id { get; set; }
    public int MaterielId { get; set; }
    public string MaterielNom { get; set; } = string.Empty;
    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public string Motif { get; set; } = string.Empty;
    public string Statut { get; set; } = "PLANIFIEE";
    public DateTime DateCreation { get; set; }
}
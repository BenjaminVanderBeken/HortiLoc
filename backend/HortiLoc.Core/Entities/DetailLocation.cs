namespace HortiLoc.Core.Entities;

public class DetailLocation
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public int MaterielId { get; set; }
    public string MaterielNom { get; set; } = string.Empty;
    public int Quantite { get; set; }
    public decimal PrixJournalier { get; set; }
    public decimal SousTotal { get; set; }
}
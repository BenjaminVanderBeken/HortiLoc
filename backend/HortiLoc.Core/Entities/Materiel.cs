namespace HortiLoc.Core.Entities;

public class Materiel
{
    public int Id { get; set; }
    public int CategorieId { get; set; }
    public string CategorieNom { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal PrixJournalier { get; set; }
    public int QuantiteTotale { get; set; }
    public int QuantiteDisponible { get; set; }
    public bool Actif { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime DateModification { get; set; }
}
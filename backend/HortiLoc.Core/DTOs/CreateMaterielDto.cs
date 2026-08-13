namespace HortiLoc.Core.DTOs;

public class CreateMaterielDto
{
    public int CategorieId { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PrixJournalier { get; set; }
    public int QuantiteTotale { get; set; }
}
namespace HortiLoc.Core.DTOs;

public class UpdateMaintenanceDto
{
    public int MaterielId { get; set; }
    public DateTime DateDebut { get; set; }
    public string Motif { get; set; } = string.Empty;
}
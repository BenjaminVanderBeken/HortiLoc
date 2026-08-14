namespace HortiLoc.Core.DTOs;

public class CreateLocationDto
{
    public int ClientId { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFinPrevue { get; set; }
    public string? Notes { get; set; }
    public List<CreateDetailLocationDto> Details { get; set; } = [];
}
using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;

namespace HortiLoc.Core.Services;

public class MaterielService
{
    private readonly IMaterielRepository _materielRepository;

    public MaterielService(IMaterielRepository materielRepository)
    {
        _materielRepository = materielRepository;
    }

    public async Task<IEnumerable<Materiel>> GetAllAsync()
    {
        return await _materielRepository.GetAllAsync();
    }

    public async Task<Materiel?> GetByIdAsync(int id)
    {
        return await _materielRepository.GetByIdAsync(id);
    }

    public async Task<Materiel> CreateAsync(CreateMaterielDto dto)
    {
        Valider(
            dto.CategorieId,
            dto.Nom,
            dto.PrixJournalier,
            dto.QuantiteTotale
        );

        var materiel = new Materiel
        {
            CategorieId = dto.CategorieId,
            Nom = dto.Nom.Trim(),
            Description = dto.Description?.Trim(),
            ImageUrl = dto.ImageUrl?.Trim(),
            PrixJournalier = dto.PrixJournalier,
            QuantiteTotale = dto.QuantiteTotale,
            QuantiteDisponible = dto.QuantiteTotale,
            Actif = true
        };

        materiel.Id =
            await _materielRepository.CreateAsync(materiel);

        return materiel;
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateMaterielDto dto
    )
    {
        var materiel =
            await _materielRepository.GetByIdAsync(id);

        if (materiel is null)
            return false;

        Valider(
            dto.CategorieId,
            dto.Nom,
            dto.PrixJournalier,
            dto.QuantiteTotale
        );

        int quantiteLouee =
            materiel.QuantiteTotale
            - materiel.QuantiteDisponible;

        if (dto.QuantiteTotale < quantiteLouee)
        {
            throw new InvalidOperationException(
                "La quantité totale ne peut pas être inférieure au nombre de matériels actuellement loués."
            );
        }

        materiel.CategorieId = dto.CategorieId;
        materiel.Nom = dto.Nom.Trim();
        materiel.Description = dto.Description?.Trim();
        materiel.ImageUrl = dto.ImageUrl?.Trim();
        materiel.PrixJournalier = dto.PrixJournalier;
        materiel.QuantiteTotale = dto.QuantiteTotale;
        materiel.QuantiteDisponible =
            dto.QuantiteTotale - quantiteLouee;

        return await _materielRepository.UpdateAsync(materiel);
    }

    public async Task<bool> DisableAsync(int id)
    {
        return await _materielRepository.DisableAsync(id);
    }

    public async Task<bool> ReactivateAsync(int id)
    {
        return await _materielRepository.ReactivateAsync(id);
    }

    private static void Valider(
        int categorieId,
        string nom,
        decimal prixJournalier,
        int quantiteTotale)
    {
        if (categorieId <= 0)
            throw new ArgumentException(
                "La catégorie est obligatoire."
            );

        if (string.IsNullOrWhiteSpace(nom))
            throw new ArgumentException(
                "Le nom du matériel est obligatoire."
            );

        if (prixJournalier < 0)
            throw new ArgumentException(
                "Le prix journalier ne peut pas être négatif."
            );

        if (quantiteTotale <= 0)
            throw new ArgumentException(
                "La quantité totale doit être supérieure à zéro."
            );
    }
}
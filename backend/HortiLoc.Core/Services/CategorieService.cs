using HortiLoc.Core.DTOs;
using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;

namespace HortiLoc.Core.Services;

public class CategorieService
{
    private readonly ICategorieRepository _categorieRepository;

    public CategorieService(ICategorieRepository categorieRepository)
    {
        _categorieRepository = categorieRepository;
    }

    public Task<IEnumerable<Categorie>> GetAllAsync()
    {
        return _categorieRepository.GetAllAsync();
    }

    public Task<Categorie?> GetByIdAsync(int id)
    {
        return _categorieRepository.GetByIdAsync(id);
    }

    public async Task<Categorie> CreateAsync(CreateCategorieDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nom))
            throw new ArgumentException("Le nom de la catégorie est obligatoire.");

        var nom = dto.Nom.Trim();

        if (await _categorieRepository.ExistsByNomAsync(nom))
            throw new InvalidOperationException(
                "Une catégorie avec ce nom existe déjà."
            );

        var categorie = new Categorie
        {
            Nom = nom,
            Description = dto.Description?.Trim(),
            Actif = true
        };

        categorie.Id = await _categorieRepository.CreateAsync(categorie);

        return await _categorieRepository.GetByIdAsync(categorie.Id)
            ?? throw new InvalidOperationException(
                "Impossible de récupérer la catégorie après sa création."
            );
    }

    public async Task<Categorie?> UpdateAsync(
        int id,
        UpdateCategorieDto dto)
    {
        var categorie = await _categorieRepository.GetByIdAsync(id);

        if (categorie is null)
            return null;

        if (string.IsNullOrWhiteSpace(dto.Nom))
            throw new ArgumentException("Le nom de la catégorie est obligatoire.");

        var nom = dto.Nom.Trim();

        if (await _categorieRepository.ExistsByNomAsync(nom, id))
            throw new InvalidOperationException(
                "Une catégorie avec ce nom existe déjà."
            );

        categorie.Nom = nom;
        categorie.Description = dto.Description?.Trim();

        await _categorieRepository.UpdateAsync(categorie);

        return await _categorieRepository.GetByIdAsync(id);
    }

    public Task<bool> DisableAsync(int id)
    {
        return _categorieRepository.DisableAsync(id);
    }

    public Task<bool> ReactivateAsync(int id)
    {
        return _categorieRepository.ReactivateAsync(id);
    }
}
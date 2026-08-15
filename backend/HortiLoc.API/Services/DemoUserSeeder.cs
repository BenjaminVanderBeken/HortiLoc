using HortiLoc.Core.Entities;
using HortiLoc.Core.Interfaces;

namespace HortiLoc.API.Services;

public static class DemoUserSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var utilisateurRepository =
            scope.ServiceProvider.GetRequiredService<IUtilisateurRepository>();

        var passwordService =
            scope.ServiceProvider.GetRequiredService<IPasswordService>();

        var clientRepository =
            scope.ServiceProvider.GetRequiredService<IClientRepository>();

        await CreerAdminAsync(
            utilisateurRepository,
            passwordService
        );

        await CreerClientAsync(
            utilisateurRepository,
            passwordService,
            clientRepository
        );
    }

    private static async Task CreerAdminAsync(
        IUtilisateurRepository utilisateurRepository,
        IPasswordService passwordService)
    {
        const string email = "admin@hortiloc.be";

        if (await utilisateurRepository.GetByEmailAsync(email) is not null)
            return;

        var utilisateur = new Utilisateur
        {
            Email = email,
            Role = "ADMIN",
            Actif = true
        };

        utilisateur.MotDePasseHash =
            passwordService.HashPassword(
                utilisateur,
                "Admin123!"
            );

        await utilisateurRepository.CreateAsync(utilisateur);
    }

    private static async Task CreerClientAsync(
        IUtilisateurRepository utilisateurRepository,
        IPasswordService passwordService,
        IClientRepository clientRepository)
    {
        const string email = "client@hortiloc.be";

        if (await utilisateurRepository.GetByEmailAsync(email) is not null)
            return;

        var client = await clientRepository.GetByIdAsync(1);

        if (client is null)
            return;

        var utilisateur = new Utilisateur
        {
            ClientId = client.Id,
            Email = email,
            Role = "CLIENT",
            Actif = true
        };

        utilisateur.MotDePasseHash =
            passwordService.HashPassword(
                utilisateur,
                "Client123!"
            );

        await utilisateurRepository.CreateAsync(utilisateur);
    }
}
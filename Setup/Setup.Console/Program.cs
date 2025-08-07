using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using Microsoft.Extensions.Configuration;
using Permission.Contracts.Requests;
using Refit;


public interface IAuthApiClient
{
    [Post("/api/authentication/register")]
    Task<RegisterUserResponse> RegisterAsync([Body] RegisterUserRequest request);
}

public interface ISetupApiClient
{
    [Post("/api/internal/setup/initial-admin")]
    Task<string> SeedInitialAdminAsync([Body] SeedInitialAdminRequest request);
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var publicApiUrl = config["ApiGatewayUrl"];
        var internalApiUrl = "http://localhost:5293"; // Die direkte URL des internen Gateways
        var adminEmail = config["AdminUser:Email"];
        var adminPassword = config["AdminUser:Password"];
        var adminRoleName = config["SystemAdminRoleName"];
        Guid adminRoleId = Guid.Parse(config["SystemAdminRoleId"]);

        Console.WriteLine("Setup-Prozess wird gestartet...");

        var authApi = RestService.For<IAuthApiClient>(publicApiUrl);
        var setupApi = RestService.For<ISetupApiClient>(internalApiUrl); // Nutzt das interne Gateway

        try
        {
            // --- Schritt 1: Admin-Benutzer über das öffentliche Gateway registrieren ---
            Console.WriteLine($"Registriere Admin-Benutzer: {adminEmail}...");
            var registerResponse = await authApi.RegisterAsync(new RegisterUserRequest(adminEmail, adminPassword));
            var userId = registerResponse.UserId;
            Console.WriteLine($"Benutzer erfolgreich mit ID {userId} registriert.");

            // --- Schritt 2: Rolle über das interne Gateway zuweisen ---
            Console.WriteLine($"Weise die Rolle '{adminRoleName}' über den internen Setup-Endpunkt zu...");
            var seedRequest = new SeedInitialAdminRequest(userId, adminRoleId, "global");
            var setupResponse = await setupApi.SeedInitialAdminAsync(seedRequest);
            Console.WriteLine($"Antwort vom Permission-Service: {setupResponse}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Setup erfolgreich abgeschlossen!");
            Console.ResetColor();
        }
        catch (ApiException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ein API-Fehler ist aufgetreten: {ex.StatusCode}");
            Console.WriteLine($"Antwort: {ex.Content}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {ex.Message}");
            Console.ResetColor();
        }
    }
}

# --- Konfiguration ---
$dbUser = "postgres"
$dbPassword = "DEIN_SICHERES_PASSWORT" # �ndern!
$solutionPath = "C:\Pfad\zu\deinem\Projekt\STAR" # �ndern!

# --- Schritt 1: Firewall-Regeln (Beispiel) ---
# F�hre dies einmalig als Administrator aus, um die Ports zu �ffnen
# New-NetFirewallRule -DisplayName "PostgreSQL" -Direction Inbound -Protocol TCP -LocalPort 5432 -Action Allow
# New-NetFirewallRule -DisplayName "RabbitMQ" -Direction Inbound -Protocol TCP -LocalPort 5672,15672 -Action Allow

# --- Schritt 2: Datenbanken erstellen ---
Write-Host "Erstelle Datenbanken..."
psql -U $dbUser -c "CREATE DATABASE AuthDb;"
psql -U $dbUser -c "CREATE DATABASE SessionDb;"
psql -U $dbUser -c "CREATE DATABASE organizationDb;"
psql -U $dbUser -c "CREATE DATABASE PermissionDb;"
psql -U $dbUser -c "CREATE DATABASE AttendanceDb;"
psql -U $dbUser -c "CREATE DATABASE CostObjectDb;"
psql -U $dbUser -c "CREATE DATABASE PlanningDb;"
psql -U $dbUser -c "CREATE DATABASE TimeTrackingDb;"

# --- Schritt 3: Migrationen ausf�hren ---
Write-Host "F�hre Datenbank-Migrationen aus..."
cd "$solutionPath\src\Services\Auth\Auth.Api"
dotnet ef database update
cd "$solutionPath\src\Services\Session\Session.Api"
dotnet ef database update
cd "$solutionPath\src\Services\Organization\Organization.Api"
dotnet ef database update
cd "$solutionPath\src\Services\Permission\Permission.Api"
dotnet ef database update
# ... f�ge hier die restlichen Services hinzu ...

# --- Schritt 4: Gateways starten ---
Write-Host "Starte die API-Gateways..."
Start-Process -FilePath "$solutionPath\src\Gateways\InternalApiGateway\bin\Release\net8.0\InternalApiGateway.exe"
Start-Process -FilePath "$solutionPath\src\Gateways\ApiGateway\bin\Release\net8.0\ApiGateway.exe"
Start-Sleep -Seconds 10

# --- Schritt 5: Microservices starten ---
# Hier musst du die Pfade zu deinen kompilierten .exe-Dateien angeben
Write-Host "Starte Microservices..."
Start-Process -FilePath "$solutionPath\src\Services\Permission\Permission.Api\bin\Release\net8.0\Permission.Api.exe"
Start-Sleep -Seconds 10
Start-Process -FilePath "$solutionPath\src\Services\Auth\Auth.Api\bin\Release\net8.0\Auth.Api.exe"
Start-Sleep -Seconds 10
Start-Process -FilePath "$solutionPath\src\Services\Organization\Organization.Api\bin\Release\net8.0\Organization.Api.exe"
Start-Sleep -Seconds 10
Start-Process -FilePath "$solutionPath\src\Services\Session\Session.Api\bin\Release\net8.0\Session.Api.exe"
Start-Sleep -Seconds 10
# ... f�ge hier die restlichen Services hinzu ...

# --- Schritt 6: Initialen Admin-Benutzer erstellen & Rolle zuweisen ---
Write-Host "Erstelle initialen Admin-Benutzer..."
cd "$solutionPath\Setup\Setup.Console"
dotnet run

Write-Host "Setup abgeschlossen!"
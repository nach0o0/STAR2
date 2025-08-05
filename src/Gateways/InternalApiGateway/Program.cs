var builder = WebApplication.CreateBuilder(args);

// Weist den Server an, nur auf localhost an einem bestimmten Port zu lauschen.
// So ist er aus dem Netzwerk nicht erreichbar.
builder.WebHost.UseUrls("http://localhost:5293");

// Lade die YARP-Konfiguration und füge den Reverse Proxy hinzu.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Aktiviere die Reverse-Proxy-Middleware.
app.MapReverseProxy();

app.Run();

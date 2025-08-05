var builder = WebApplication.CreateBuilder(args);

// 1. Lade die YARP-Konfiguration und füge den Reverse Proxy hinzu.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Lese alle Cluster-Definitionen aus der YARP-Konfiguration.
    var clusters = builder.Configuration.GetSection("ReverseProxy:Clusters").GetChildren();

    // Erstelle für jeden gefundenen Cluster dynamisch eine Swagger-Dokumentation.
    foreach (var cluster in clusters)
    {
        var clusterName = cluster.Key.Replace("-cluster", "", StringComparison.OrdinalIgnoreCase);
        options.SwaggerDoc(clusterName, new() { Title = $"{clusterName.ToUpperInvariant()} Service", Version = "v1" });
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Lese erneut alle Cluster-Definitionen.
        var clusters = builder.Configuration.GetSection("ReverseProxy:Clusters").GetChildren();

        // Erstelle für jeden Cluster dynamisch einen Endpunkt in der Swagger-UI.
        foreach (var cluster in clusters)
        {
            var clusterName = cluster.Key.Replace("-cluster", "", StringComparison.OrdinalIgnoreCase);
            // Annahme: Die Route in appsettings.json ist immer "/swagger/{clusterName}/..."
            options.SwaggerEndpoint($"/swagger/{clusterName}/swagger/v1/swagger.json", $"{clusterName.ToUpperInvariant()} Service");
        }
    });
}

// 2. Aktiviere die Reverse-Proxy-Middleware.
app.MapReverseProxy();

app.Run();

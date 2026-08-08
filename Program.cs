using GreenForest.AI;
using GreenForest.Data;
using GreenForest.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

AppContext.SetSwitch(
    "Npgsql.EnableLegacyTimestampBehavior",
    true
);

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// BASE DE DATOS - SUPABASE / POSTGRESQL
// ============================================================

var connectionString =
    builder.Configuration.GetConnectionString(
        "DefaultConnection"
    );

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception(
        "La cadena de conexión DefaultConnection está vacía."
    );
}

builder.Services.AddDbContext<GreenForestContext>(
    options =>
        options.UseNpgsql(connectionString)
);

// ============================================================
// CONTROLLERS
// ============================================================

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// ============================================================
// SERVICIOS
// ============================================================

builder.Services.AddScoped<UsuarioService>();

builder.Services.AddHttpClient<GeminiService>();

// ============================================================
// SWAGGER
// ============================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// ============================================================
// CREAR APLICACIÓN
// ============================================================

var app = builder.Build();

// ============================================================
// MIGRACIONES Y DATOS INICIALES
// ============================================================

using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<GreenForestContext>();

    context.Database.Migrate();

    DbSeeder.Seed(context);
}

// ============================================================
// SWAGGER
// ============================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

// ============================================================
// MIDDLEWARE
// ============================================================

app.UseAuthorization();

app.MapControllers();

app.Run();
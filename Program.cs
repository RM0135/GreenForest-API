using GreenForest.Data;
using GreenForest.Services;
using Microsoft.EntityFrameworkCore;
using GreenForest.AI;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GreenForestContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// SQLite
builder.Services.AddDbContext<GreenForestContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios
builder.Services.AddScoped<UsuarioService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddControllers();

var app = builder.Build();

// Sembrar datos de ejemplo si la base está vacía
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GreenForestContext>();
    context.Database.EnsureCreated();
    DbSeeder.Seed(context);
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
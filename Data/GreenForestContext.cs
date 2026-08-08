using GreenForest.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenForest.Data;

public class GreenForestContext : DbContext
{
    public GreenForestContext(DbContextOptions<GreenForestContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Proyecto> Proyectos { get; set; }

    public DbSet<Arbol> Arboles { get; set; }

    public DbSet<Reporte> Reportes { get; set; }

    public DbSet<Especie> Especies { get; set; }

    public DbSet<Organizacion> Organizaciones { get; set; }
}
using GreenForest.Models;

namespace GreenForest.Data;

public static class DbSeeder
{
    public static void Seed(GreenForestContext context)
    {
        // Si ya hay usuarios, asumimos que la base ya tiene datos y no volvemos a sembrar.
        if (context.Usuarios.Any())
            return;

        var usuario = new Usuario
        {
            Nombre = "Juan Pablo",
            Apellido = "Ramirez",
            Correo = "juanpi@greenforest.co",
            Password = "hashed_pw_demo",
            Rol = "Administrador"
        };
        context.Usuarios.Add(usuario);

        var organizacion = new Organizacion
        {
            Nombre = "Fundación Bosques Vivos",
            Tipo = "ONG",
            Telefono = "3001234567",
            Correo = "contacto@bosquesvivos.org"
        };
        context.Organizaciones.Add(organizacion);

        var ceiba = new Especie 
        {
        NombreComun = "Ceiba",
        NombreCientifico = "Ceiba pentandra",
        DistanciaSiembra = 5.0m,
        ClimaIdeal = "Tropical cálido",
        TemperaturaMinima = 20,
        TemperaturaMaxima = 32,
        PrecipitacionMinima = 1000,
        PrecipitacionMaxima = 3000,
        HumedadMinima = 50,
        HumedadMaxima = 85,
        TipoSuelo = "Bien drenado",
        ToleranciaSequía = "Media"
       
        };
        var guayacan = new Especie
        {
        NombreComun = "Guayacan",
        NombreCientifico = "Handroanthus chrysanthus",
        DistanciaSiembra = 4.0m,
        ClimaIdeal = "Tropical seco",
        TemperaturaMinima = 18,
        TemperaturaMaxima = 30,
        PrecipitacionMinima = 700,
        PrecipitacionMaxima = 1800,
        HumedadMinima = 50,
        HumedadMaxima = 85,
        TipoSuelo = "Bien drenado",
        ToleranciaSequía = "Alta"
        };
        context.Especies.AddRange(ceiba, guayacan);

        context.SaveChanges(); // Guardamos primero para tener los Ids generados

        var proyecto = new Proyecto
        {
            Nombre = "Reforestacion Rio Frio",
            Descripcion = "Recuperacion de ronda hidrica",
            FechaInicio = new DateTime(2026, 1, 15),
            FechaFin = new DateTime(2026, 12, 15),
            Estado = "Activo",
            MetaArboles = 500,
            MetaHectareas = 8.5m,
            UsuarioId = usuario.Id,
            OrganizacionId = organizacion.Id
        };
        context.Proyectos.Add(proyecto);
        context.SaveChanges();

        var arbol = new Arbol
        {
            FechaSiembra = new DateTime(2026, 2, 1),
            Estado = "Sembrado",
            Altura = 1.2m,
            Latitud = 10.9878m,
            Longitud = -74.9547m,
            ProyectoId = proyecto.Id,
            EspecieId = ceiba.Id
        };
        context.Arboles.Add(arbol);

        var reporte = new Reporte
        {
            FechaGeneracion = new DateTime(2026, 3, 1),
            Descripcion = "Avance mes 1",
            CantidadArboles = 120,
            Observaciones = "Buen desarrollo, sin mortalidad",
            ProyectoId = proyecto.Id
        };
        context.Reportes.Add(reporte);

        context.SaveChanges();
    }
}

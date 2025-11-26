

using DotNetEnv;
using DSW1_T1_GOMEZ_JHONATAN.Data;
using DSW1_T1_GOMEZ_JHONATAN.Models;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);


// Cade conexion

var server = Environment.GetEnvironmentVariable("DB_SERVER");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString = $"Server={server};Port={port};Database={database};User={user};Password={password}";

// Add services to the container.
builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configurar JSON para evitar ciclos de referencia
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true; // JSON más legible
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Asegurar que la base de datos esté creada automáticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BibliotecaContext>();
    context.Database.EnsureCreated(); // Esto creará las tablas automáticamente

    // Agregar datos iniciales si las tablas están vacías
    if (!context.NivelesAcademicos.Any())
    {
        context.NivelesAcademicos.AddRange(
            new NivelAcademico { Descripcion = "Primer Año", Orden = 1 },
            new NivelAcademico { Descripcion = "Segundo Año", Orden = 2 },
            new NivelAcademico { Descripcion = "Tercer Año", Orden = 3 }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

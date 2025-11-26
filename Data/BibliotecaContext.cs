using DSW1_T1_GOMEZ_JHONATAN.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW1_T1_GOMEZ_JHONATAN.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options)
        {

        }

        // Entidades del sistema académico
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<NivelAcademico> NivelesAcademicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones para el sistema académico
            // Una NivelAcademico tiene muchos Cursos
            modelBuilder.Entity<Curso>()
                .HasOne(c => c.NivelAcademico)
                .WithMany(n => n.Cursos)
                .HasForeignKey(c => c.NivelAcademicoId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
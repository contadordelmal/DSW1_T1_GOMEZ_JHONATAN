using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSW1_T1_GOMEZ_JHONATAN.Models
{
    [Table("cursos")]
    public class Curso
    {
        [Key]
        [Column("curso_id")]
        public int CursoId { get; set; }

        [Required(ErrorMessage = "El código del curso es obligatorio")]
        [StringLength(20)]
        [Column("codigo_curso")]
        public string CodigoCurso { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del curso es obligatorio")]
        [StringLength(100)]
        [Column("nombre_curso")]
        public string NombreCurso { get; set; } = string.Empty;

        [Required]
        [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10")]
        [Column("creditos")]
        public int Creditos { get; set; }

        [Required]
        [Range(1, 40, ErrorMessage = "Las horas semanales deben estar entre 1 y 40")]
        [Column("horas_semanales")]
        public int HorasSemanales { get; set; }

        // Clave foránea hacia NivelAcademico
        [Required]
        [Column("nivel_academico_id")]
        public int NivelAcademicoId { get; set; }

        // Propiedad de navegación hacia NivelAcademico
        [ForeignKey("NivelAcademicoId")]
        public virtual NivelAcademico? NivelAcademico { get; set; }
    }
}

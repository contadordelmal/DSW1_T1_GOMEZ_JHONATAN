using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSW1_T1_GOMEZ_JHONATAN.Models
{
    [Table("niveles_academicos")]
    public class NivelAcademico
    {
        [Key]
        [Column("nivel_academico_id")]
        public int NivelAcademicoId { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(100)]
        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Column("orden")]
        public int Orden { get; set; }

        // Relación uno a muchos: Un nivel académico tiene muchos cursos
        public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();
    }
}

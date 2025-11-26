using DSW1_T1_GOMEZ_JHONATAN.Data;
using DSW1_T1_GOMEZ_JHONATAN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DSW1_T1_GOMEZ_JHONATAN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        // b. Inyectar el DbContext en el constructor
        public CursosController(BibliotecaContext context)
        {
            _context = context;
        }

        // c. Implementar un endpoint GET para listar todos los cursos con sus niveles académicos incluidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            return await _context.Cursos
                .Include(c => c.NivelAcademico)
                .ToListAsync();
        }

        // d. Implementar un endpoint GET para obtener los cursos por nivel académico
        // Método específico requerido: ListarCursosPorNivel
        // Consulta la base de datos usando LINQ para filtrar por nivel académico
        [HttpGet("PorNivel/{nivelAcademicoId}")]
        public async Task<ActionResult<IEnumerable<Curso>>> ListarCursosPorNivel(int nivelAcademicoId)
        {
            var cursos = await _context.Cursos
                .Include(c => c.NivelAcademico)
                .Where(c => c.NivelAcademicoId == nivelAcademicoId)
                .ToListAsync();

            if (!cursos.Any())
            {
                return NotFound($"No se encontraron cursos para el nivel académico ID {nivelAcademicoId}");
            }

            return cursos;
        }

        // e. Implementar un endpoint POST para crear un nuevo curso
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            // Validar que el nivel académico existe
            var nivelAcademico = await _context.NivelesAcademicos
                .FindAsync(curso.NivelAcademicoId);

            if (nivelAcademico == null)
            {
                return BadRequest($"No existe el nivel académico con ID {curso.NivelAcademicoId}");
            }

            // Validar que el código del curso sea único
            var cursoExistente = await _context.Cursos
                .FirstOrDefaultAsync(c => c.CodigoCurso == curso.CodigoCurso);

            if (cursoExistente != null)
            {
                return BadRequest($"Ya existe un curso con el código {curso.CodigoCurso}");
            }

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            // Incluir el nivel académico en la respuesta
            await _context.Entry(curso)
                .Reference(c => c.NivelAcademico)
                .LoadAsync();

            return CreatedAtAction(nameof(GetCursos), new { id = curso.CursoId }, curso);
        }

        // f. Implementar un endpoint PUT para actualizar un curso existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {
            if (id != curso.CursoId)
            {
                return BadRequest("El ID del curso no coincide con el ID de la URL");
            }

            // Verificar que el curso existe
            var cursoExistente = await _context.Cursos.FindAsync(id);
            if (cursoExistente == null)
            {
                return NotFound($"No se encontró el curso con ID {id}");
            }

            // Validar que el nivel académico existe
            var nivelAcademico = await _context.NivelesAcademicos
                .FindAsync(curso.NivelAcademicoId);

            if (nivelAcademico == null)
            {
                return BadRequest($"No existe el nivel académico con ID {curso.NivelAcademicoId}");
            }

            // Validar que el código del curso sea único (excluyendo el curso actual)
            var cursoConMismoCodigo = await _context.Cursos
                .FirstOrDefaultAsync(c => c.CodigoCurso == curso.CodigoCurso && c.CursoId != id);

            if (cursoConMismoCodigo != null)
            {
                return BadRequest($"Ya existe otro curso con el código {curso.CodigoCurso}");
            }

            // Actualizar propiedades
            cursoExistente.CodigoCurso = curso.CodigoCurso;
            cursoExistente.NombreCurso = curso.NombreCurso;
            cursoExistente.Creditos = curso.Creditos;
            cursoExistente.HorasSemanales = curso.HorasSemanales;
            cursoExistente.NivelAcademicoId = curso.NivelAcademicoId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.CursoId == id);
        }
    }
}

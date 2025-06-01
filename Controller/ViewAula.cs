using API_Cursos.Data;
using API_Cursos.DTOs;
using API_Cursos.Entities;
using API_Cursos.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Cursos.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewAula : ControllerBase
    {
        private readonly AppDBContext _context;

        public ViewAula(AppDBContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AulaDTO>>> GetAula([FromQuery] bool activo = true)
        {
            return await _context.ProfesorCurso
            .Include(aula => aula.Profesor)
            .Include(aula => aula.Curso)
            .Include(aula => aula.Asignacion)
                .ThenInclude(asign => asign.Estudiante)
            .Where(aula => aula.Estado == activo)
            .Select(
                aula => new AulaDTO
                {
                    IdAula = aula.IdProfesorCurso,
                    Curso = new CursoDTO
                    {
                        idCurso = aula.Curso!.IdCurso,
                        Nombre = aula.Curso.Nombre
                    },
                    Profesor = new ProfesorSampleDTO
                    {
                        IdProfesor = aula.Profesor!.IdProfesor,
                        Nombre = aula.Profesor.Nombre,
                        CapacidadEstudiantes = aula.Profesor.CapacidadEstudiantes
                    },
                    TotalEstudiantes = aula.Asignacion.Where(asig => asig.Estado == true).Count(),
                    Estado = aula.Estado

                }
            )
            .OrderByDescending(aula => aula.TotalEstudiantes)
            .ToListAsync();
            

        }
        
    }
}

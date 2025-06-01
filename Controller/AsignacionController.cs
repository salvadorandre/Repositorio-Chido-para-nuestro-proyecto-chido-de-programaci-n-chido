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
    public class AsignacionController : ControllerBase
    {
        private readonly AppDBContext _context;

        public AsignacionController(AppDBContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AsigancionDetalleDTO>>> GetAsignacion([FromQuery] bool activo = true)
        {
            return await _context
            .Asignacion
            .Include(estudiante => estudiante.Estudiante)
            .Include(asignacionProfesor => asignacionProfesor.ProfesorCurso)
                .ThenInclude(pc => pc!.Curso)
            .Include(asignacionProfesor => asignacionProfesor.ProfesorCurso)
                .ThenInclude(pc => pc!.Profesor)
            .Where(asignacion => asignacion.Estado == activo)
            .Select(
                asignacion =>
                new AsigancionDetalleDTO
                {
                    IdAsignacion = asignacion.IDAsignacion,
                    Fecha = asignacion.Fecha,
                    Estudiante = new EstudianteDTO
                    {
                        IdEstudiante = asignacion.Estudiante!.IdEstudiante,
                        Nombre = asignacion.Estudiante.Nombre,
                        Apellido = asignacion.Estudiante.Apellido,

                    },
                    Curso = new CursoDTO
                    {
                        idCurso = asignacion.ProfesorCurso!.Curso!.IdCurso,
                        Nombre = asignacion.ProfesorCurso.Curso.Nombre
                    },
                    Profesor = new ProfesorDTO
                    {
                        IdProfesor = asignacion.ProfesorCurso!.Profesor!.IdProfesor,
                        Nombre = asignacion.ProfesorCurso!.Profesor!.Nombre,
                        CapacidadEstudiantes = asignacion.ProfesorCurso!.Profesor!.CapacidadEstudiantes,
                        Estado = asignacion.ProfesorCurso.Profesor.Estado
                    },
                    Estado = asignacion.Estado


                }
            ).ToListAsync();

        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AsigancionDetalleDTO>> GetAsignacion([FromRoute] int id)
        {
            var asignacion = await _context
            .Asignacion
            .Include(estudiante => estudiante.Estudiante)
            .Include(asignacionProfesor => asignacionProfesor.ProfesorCurso)
                .ThenInclude(pc => pc!.Curso)
            .Include(asignacionProfesor => asignacionProfesor.ProfesorCurso)
                .ThenInclude(pc => pc!.Profesor)
            .FirstOrDefaultAsync(asignacion => asignacion.IDAsignacion == id);

            if (asignacion == null)
            {
                return NotFound();
            }

            return
                Ok(
                   new AsigancionDetalleDTO
                   {
                       IdAsignacion = asignacion.IDAsignacion,
                       Fecha = asignacion.Fecha,
                       Estudiante = new EstudianteDTO
                       {
                           IdEstudiante = asignacion.Estudiante!.IdEstudiante,
                           Nombre = asignacion.Estudiante.Nombre,
                           Apellido = asignacion.Estudiante.Apellido,

                       },
                       Curso = new CursoDTO
                       {
                           idCurso = asignacion.ProfesorCurso!.Curso!.IdCurso,
                           Nombre = asignacion.ProfesorCurso.Curso.Nombre
                       },
                       Profesor = new ProfesorDTO
                       {
                           IdProfesor = asignacion.ProfesorCurso!.Profesor!.IdProfesor,
                           Nombre = asignacion.ProfesorCurso!.Profesor!.Nombre,
                           CapacidadEstudiantes = asignacion.ProfesorCurso!.Profesor!.CapacidadEstudiantes,
                           Estado = asignacion.ProfesorCurso.Profesor.Estado
                       },
                       Estado = asignacion.Estado


                   });
        }

        [HttpPost]
        public async Task<ActionResult> PostAsignacion([FromBody] Asignacion asignacion)
        {
            try
            {
                if (asignacion == null)
                {
                    ModelState
                        .AddModelError("asignacion", $"Todos los datos son requeridos");

                    return ValidationProblem();
                }
                var asignacionExisistente = await _context.Asignacion
                .Where(a => a.Estado == true)
                .AnyAsync(a =>
                a.ProfesorCursoId == asignacion.ProfesorCursoId
                && a.EstudianteId == asignacion.EstudianteId);

                if (asignacionExisistente)
                {
                    ModelState
                        .AddModelError("asignacion", $"Ya existe una asignacion existente");

                    return ValidationProblem();
                }

                var profesorAsignado = await _context
                .ProfesorCurso
                .Include(profesor => profesor.Profesor)
                .Include(asign => asign.Asignacion)
                .FirstOrDefaultAsync(prof => prof.IdProfesorCurso == asignacion.ProfesorCursoId);
                var estudiante = await _context
                .Estudiante
                .FirstOrDefaultAsync(es => es.IdEstudiante == asignacion.EstudianteId);

                if (profesorAsignado == null || estudiante == null)
                {
                    ModelState.AddModelError(nameof(asignacion), "ProfesorCurso o Estudiante no encontrados");
                    return ValidationProblem();
                }

                if (!profesorAsignado.Estado || !estudiante.Estado)
                {
                    ModelState.AddModelError(nameof(asignacion), "El ProfesorCurso y el Estudiante deben estar activos");
                    return ValidationProblem();
                }
                if (estudiante.Promedio<61)
                {
                    ModelState.AddModelError(nameof(asignacion), "El promedio del estudiante debe ser mayor o igual a 61");
                    return ValidationProblem();
                }

                var totalAsignaciones = profesorAsignado.Asignacion.Count(a => a.Estado);
                var capacidadProfesor = profesorAsignado?.Profesor?.CapacidadEstudiantes;

                if (capacidadProfesor <= totalAsignaciones)
                {
                    ModelState
                        .AddModelError("asignacion", $"No fue posible asignar mas estudiantes a este profesor");

                    return ValidationProblem();
                }

                asignacion.Fecha = DateTime.UtcNow;
                _context.Add(asignacion);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Insertado correctamente"
                });
            }
            catch (System.Exception ex)
            {

                ModelState.AddModelError(nameof(asignacion), "Ocurrió un error inesperado al guardar la asignación");
                return ValidationProblem();
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsignacion([FromRoute] int id, [FromBody] Asignacion asignacion)
        {

            if (asignacion == null || asignacion.IDAsignacion != id)
            {
                return BadRequest("Datos Inconcistentes o nulos");
            }
            var asignacionExistente = await _context.Asignacion.FindAsync(id);
            if (asignacionExistente == null)
            {
                return NotFound("asignacion no encontrado");
            }

            asignacion.Fecha = DateTime.UtcNow;
            _context.Entry(asignacionExistente).CurrentValues.SetValues(asignacion);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Actualizado correctamente"
            });

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DisableAsignacion([FromRoute] int id = 0)
        {
            if (id == 0)
            {
                return BadRequest("Datos Inconcistentes o nulos");
            }
            var asignacion = await _context.Asignacion.FindAsync(id);
            if (asignacion == null)
            {
                return NotFound("asignacion no encontrado");
            }
            asignacion.Fecha = DateTime.UtcNow;
            asignacion.Estado = false;
            _context.Update(asignacion);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Deshabilitado correctamente"
            });

        }
    }
}

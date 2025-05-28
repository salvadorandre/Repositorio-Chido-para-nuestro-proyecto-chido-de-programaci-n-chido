using API_Cursos.Data;
using API_Cursos.DTOs;
using API_Cursos.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Cursos.Controller
{
    [Route("api/asignacionprofesor")]
    [ApiController]
    public class ProfesorCursoController : ControllerBase
    {
        private readonly AppDBContext _context;

        public ProfesorCursoController(AppDBContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfesorCursoDTO>>> GetAsignacionProfesores([FromQuery] bool activo = true)
        {
            return Ok(await _context
            .ProfesorCurso
            .Include(curso => curso.Curso)
            .Include(profesor => profesor.Profesor)
            .Where(asignacion => asignacion.Estado == activo)
            .Select(profesorCurso =>
                new ProfesorCursoDTO
                {
                    idProfesorCurso = profesorCurso.IdProfesorCurso,
                    curso = new CursoDTO
                    {
                        idCurso = profesorCurso.Curso!.IdCurso,
                        Nombre = profesorCurso.Curso.Nombre
                    },
                    profesor = new ProfesorDTO
                    {
                        IdProfesor = profesorCurso.Profesor!.IdProfesor,
                        Nombre = profesorCurso.Profesor.Nombre,
                        CapacidadEstudiantes = profesorCurso.Profesor.CapacidadEstudiantes,
                        Edad = profesorCurso.Profesor.Edad,
                        Estado = profesorCurso.Profesor.Estado,

                    },
                    Estado = profesorCurso.Estado
                })
                .ToListAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProfesorCursoDTO>> GetAsignacionProfesor([FromRoute] int id)
        {
            var asignacion = await _context
            .ProfesorCurso
            .Include(curso => curso.Curso)
            .Include(profesor => profesor.Profesor).FirstOrDefaultAsync(profesorCuro => profesorCuro.IdProfesorCurso == id);

            if (asignacion == null)
            {
                return NotFound();
            }

            return
                Ok(
                    new ProfesorCursoDTO
                    {
                        idProfesorCurso = asignacion.IdProfesorCurso,
                        curso = new CursoDTO
                        {
                            idCurso = asignacion.Curso!.IdCurso,
                            Nombre = asignacion.Curso.Nombre
                        },
                        profesor = new ProfesorDTO
                        {
                            IdProfesor = asignacion.Profesor!.IdProfesor,
                            Nombre = asignacion.Profesor.Nombre,
                            CapacidadEstudiantes = asignacion.Profesor.CapacidadEstudiantes,
                            Edad = asignacion.Profesor.Edad,
                            Estado = asignacion.Profesor.Estado,

                        }
                        ,
                        Estado = asignacion.Estado
                    });
        }

        [HttpPost]
        public async Task<ActionResult> PostAsignacionProfesor([FromBody] ProfesorCurso profesorCurso)
        {
            try
            {
                if (profesorCurso == null)
                {
                    ModelState
                        .AddModelError("asignacion", $"Todos los datos son requeridos");

                    return ValidationProblem();
                }
                var asignacionExisistente = await _context.ProfesorCurso
                            .Where(a => a.Estado == true)
                            .AnyAsync(a =>
                            a.ProfesorId == profesorCurso.ProfesorId
                            && a.CursoId == profesorCurso.CursoId);

                if (asignacionExisistente )
                {
                    ModelState
                        .AddModelError("asignacion", $"Ya existe una asignacion existente");

                    return ValidationProblem();
                }
                var profesor = await _context.Profesor.FirstOrDefaultAsync(profesor => profesor.IdProfesor == profesorCurso.ProfesorId);
                var curso = await _context.Curso.FirstOrDefaultAsync(curso => curso.IdCurso == profesorCurso.CursoId);


                if (profesor == null || curso == null)
                {
                    ModelState
                           .AddModelError(nameof(profesorCurso), $"Curso o Profesor deben estar acitivos");

                    return ValidationProblem();
                }
                if (profesor.Estado == false || curso.Estado == false)
                {
                    ModelState
                       .AddModelError(nameof(profesorCurso), $"Curso o Profesor deben estar acitivos");

                    return ValidationProblem();
                }


                _context.Add(profesorCurso);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Insertado correctamente"
                });
            }
            catch (System.Exception)
            {

                ModelState
                        .AddModelError(nameof(profesorCurso), $"Ocurrio un problema al asignar");

                return ValidationProblem();
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsignacionProfesor([FromRoute] int id, [FromBody] ProfesorCurso profesorCurso)
        {
            if (profesorCurso == null || profesorCurso.IdProfesorCurso != id)
            {
                return BadRequest("Datos Inconcistentes o nulos");
            }
            var asignacion = await _context.ProfesorCurso.FindAsync(id);
            if (asignacion == null)
            {
                return NotFound("asignacion no encontrado");
            }

            _context.Entry(asignacion).CurrentValues.SetValues(profesorCurso);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Actualizado correctamente"
            });

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DisableAsignacionProfesor([FromRoute] int id = 0)
        {
            if (id == 0)
            {
                return BadRequest("Datos Inconcistentes o nulos");
            }
            var asignacion = await _context.ProfesorCurso.FindAsync(id);
            if (asignacion == null)
            {
                return NotFound("asignacion no encontrado");
            }
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

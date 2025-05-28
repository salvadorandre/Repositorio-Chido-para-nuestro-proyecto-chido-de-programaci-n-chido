using API_Cursos.Data;
using API_Cursos.DTOs;
using API_Cursos.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Cursos.Controller;

[Route("api/[controller]")]
[ApiController]
public class ProfesorController : ControllerBase
{
    private readonly AppDBContext _context;

    public ProfesorController(AppDBContext context)
    {
        this._context = context;
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<ProfesorDTO>>> GetAutores()
    // {
    //     return Ok(await _context
    //     .Profesor
    //     .Include(profesor => profesor.ProfesorCurso)
    //     .ThenInclude(profesorCurso => profesorCurso.Curso)
    //     .Select(
    //         profesor => new ProfesorDTO
    //         {
    //             IdProfesor = profesor.IdProfesor,
    //             Nombre = profesor.Nombre,
    //             Edad = profesor.Edad,
    //             CapacidadEstudiantes = profesor.CapacidadEstudiantes,
    //             Estado = profesor.Estado,
    //             ProfesorCurso = profesor
    //             .ProfesorCurso
    //             .Select(
    //                 profesorCursos => new ProfesorCursoDTO
    //                 {
    //                     idProfesorCurso = profesorCursos.IdProfesorCurso,
    //                     curso = new CursoDTO
    //                     {
    //                         idCurso = profesorCursos.Curso!.IdCurso,
    //                         Nombre = profesorCursos.Curso.Nombre
    //                     }

    //                 }
    //             ).ToList()
    //         }
    //     ).ToArrayAsync());
    // }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfesorDTO>>> GetAutores([FromQuery] bool activos = true)
    {
        return Ok(await _context
        .Profesor
        .Include(profesor => profesor.ProfesorCurso)
        .ThenInclude(profesorCurso => profesorCurso.Curso)
        .Where(profesor=> profesor.Estado == activos)
        .Select(
            profesor => new ProfesorDTO
            {
                IdProfesor = profesor.IdProfesor,
                Nombre = profesor.Nombre,
                Edad = profesor.Edad,
                CapacidadEstudiantes = profesor.CapacidadEstudiantes,
                Estado = profesor.Estado,
                ProfesorCurso = profesor
                .ProfesorCurso
                .Select(
                    profesorCursos => new ProfesorCursoDTO
                    {
                        idProfesorCurso = profesorCursos.IdProfesorCurso,
                        curso = new CursoDTO
                        {
                            idCurso = profesorCursos.Curso!.IdCurso,
                            Nombre = profesorCursos.Curso.Nombre
                        }

                    }
                ).ToList()
            }
        ).ToArrayAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProfesorDTO>> GetAutor([FromRoute] int id)
    {
        var profesor = await _context
        .Profesor
        .Include(profesor => profesor.ProfesorCurso)
        .ThenInclude(profesorCurso => profesorCurso.Curso)
        .FirstOrDefaultAsync(profesor =>
                profesor.IdProfesor == id
            );
        if (profesor == null)
        {
            return NotFound();
        }

        return Ok(
            new ProfesorDTO
            {
                IdProfesor = profesor.IdProfesor,
                Nombre = profesor.Nombre,
                Edad = profesor.Edad,
                CapacidadEstudiantes = profesor.CapacidadEstudiantes,
                Estado = profesor.Estado,
                ProfesorCurso = profesor
                .ProfesorCurso
                .Select(
                    profesorCursos => new ProfesorCursoDTO
                    {
                        idProfesorCurso = profesorCursos.IdProfesorCurso,
                        curso = new CursoDTO
                        {
                            idCurso = profesorCursos.Curso!.IdCurso,
                            Nombre = profesorCursos.Curso.Nombre
                        }

                    }
                ).ToList()
            }
        );
    }

    [HttpPost]
    public async Task<ActionResult> PostAutor([FromBody] Profesor profesor)
    {
        if (profesor == null)
        {
            ModelState
            .AddModelError(nameof(Profesor), $"Todos los datos son requeridos");

            return ValidationProblem();
        }

        _context.Add(profesor);
        await _context.SaveChangesAsync();

        return Ok(
            new
            {
                message = "Insertado correctamente"
            }
        );

    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> PostProfesor([FromRoute] int id, [FromBody] Profesor profesor)
    {
        if ( profesor == null || profesor.IdProfesor != id )
        {
            return BadRequest("Datos Inconcistentes o nulos");
        }
        var EProfesor = await _context.Profesor.FindAsync(id);
        if (EProfesor == null)
        {
            return NotFound("Profesor no encontrado");
        }
        _context.Entry(EProfesor).CurrentValues.SetValues(profesor);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Actualizado correctamente"
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DisableProfesor([FromRoute] int id = 0)
    {
        if (id == 0)
        {
            return BadRequest("Datos Inconcistentes o nulos");
        }
        var profesor = await _context.Profesor.FindAsync(id);
        if (profesor == null)
        {
            return NotFound("Profesor no encontrado");
        }

        profesor.Estado = false;
        _context.Update(profesor);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Deshabilitado correctamente"
        });
    }

}


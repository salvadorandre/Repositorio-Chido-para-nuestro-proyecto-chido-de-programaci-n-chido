using System.Threading.Tasks;
using API_Cursos.Data;
using API_Cursos.DTOs;
using API_Cursos.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Cursos.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class CursoController : ControllerBase { 


        public readonly AppDBContext _context;

        public CursoController(AppDBContext context) { 
            _context = context;
        }


        //Metodo para obtener todos los cursos que se encuentre activos 
        [HttpGet]
        public async Task<IActionResult> GetAllCursosActivos() { 

            var cursos = await _context.Curso.Where(e => e.Estado == true) .ToListAsync();
            return Ok(cursos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SearchCursoById(int id ) { 

            var cursoBuscado = await _context.Curso.FindAsync(id);

            if(cursoBuscado == null) { 
                return NotFound("El curso no fue encontrado.");
            }

            return Ok(cursoBuscado);
        }

        //Metodo para crear un curso 

        [HttpPost]
        public async Task<IActionResult> CreateCurso([FromBody] Curso curso) { 

            if(curso == null) { 

                return BadRequest("El curso no puede ser nulo.");
            }

            await _context.Curso.AddAsync(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllCursosActivos), new { id = curso.IdCurso }, curso);
            
        }


        [HttpPut("{id}")]
        //Actualizar un curso
        public async Task<IActionResult> UpdateCurso(int id, [FromBody] Curso newCurso) { 

            if(newCurso == null || id != newCurso.IdCurso) { 

                return BadRequest("El curso no puede ser nulo.");
            }

            var cursoOld = await _context.Curso.FindAsync(id);

            if(cursoOld == null) {
                return NotFound("El curso no fue encontrado.");
            }

            cursoOld.Nombre = newCurso.Nombre;
            cursoOld.Estado = newCurso.Estado;
            cursoOld.ProfesorCurso = newCurso.ProfesorCurso;


            await _context.SaveChangesAsync();
            return Ok(cursoOld);

        }


        [HttpDelete("{id}")]

        //Desabilitar un curso 
        public async Task<IActionResult> DisableCurso(int id) { 

            var curso = await _context.Curso.FindAsync(id); 
            if(curso == null) {
                return NotFound("El curso no fue encontrado.");
            }

            curso.Estado = false;

            await _context.SaveChangesAsync();
            return Ok("El curso fue desabilitado.");
        }





    }
}
using System.Threading.Tasks;
using API_Cursos.Data;
using API_Cursos.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Cursos.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {

        private readonly AppDBContext _context;


        //Constructor de la clase EstudianteController
        public EstudianteController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]

        //Metodo para obtener todos los estudiantes activos
        public async Task<IActionResult> GetAllEstudiantesActivos() {             

            var estudiantes = await _context.Estudiante.Where(e => e.Estado == true) .ToListAsync();
            return Ok(estudiantes);
            
        }
        [HttpPost]
        //Metodo para crear un estudiante
        public async Task<IActionResult> CreateEstudiante([FromBody] Estudiante estudiante)
        {
            if (estudiante == null)
            {
                return BadRequest("El estudiante no puede ser nulo.");
            }

            await _context.Estudiante.AddAsync(estudiante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllEstudiantesActivos), new { id = estudiante.IdEstudiante }, estudiante);
        }


        [HttpGet("{id}")]
        //Metodo para buscar un estudiante por medio de su ID
        public async Task<IActionResult> SearchEstudianteById(int id) { 
            //Buscar al estudiante por medio de su ID 
            var estudianteBuscado = await _context.Estudiante.FindAsync(id);         

            if(estudianteBuscado == null) { 
                //En caso de que no existiera retornamos el siguiente mensaje de error. 
                return NotFound("El estudiante no fue encontrado.");
            }
            //Retornamos al estudiante encontrado
            return Ok(estudianteBuscado);
        }  

        [HttpPut("{id}")]
        //Metodo para actualizar un estudiante
        public async Task<IActionResult>  UpdateEstudiante(int id, [FromBody] Estudiante estudianteActualizado) {
           
            if(estudianteActualizado == null  || id != estudianteActualizado.IdEstudiante) {
                    return BadRequest("Datos inválidos.");
            }

            var estudianteExistente = await _context.Estudiante.FindAsync(id);

            if(estudianteExistente == null) { 
                    return NotFound("El estudiante no fue encontrado.");
            }

            // Actualizar los campos del estudiante existente
            estudianteExistente.Nombre = estudianteActualizado.Nombre;
            estudianteExistente.Apellido = estudianteActualizado.Apellido;
            estudianteExistente.Promedio = estudianteActualizado.Promedio;
            estudianteExistente.Edad = estudianteActualizado.Edad;
            estudianteExistente.Grado = estudianteActualizado.Grado;
            estudianteExistente.Estado = estudianteActualizado.Estado;
            estudianteExistente.Asignacion = estudianteActualizado.Asignacion;
       
            await _context.SaveChangesAsync();

            return Ok(estudianteExistente);
        }

        [HttpDelete("{id}")]
        // Metodo para desabilitar a un estudiante
        public async Task<IActionResult> DisableEstudiante(int id) { 

            var estudiante = await _context.Estudiante.FindAsync(id);

            if (estudiante == null)
            {
                return NotFound("El estudiante no fue encontrado.");
            }

            // Cambiar el estado del estudiante a inactivo
            estudiante.Estado = false;

            await _context.SaveChangesAsync();

            return Ok(estudiante);
    
            
        }

        
    }
}

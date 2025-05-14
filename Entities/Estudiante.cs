using System;
using System.ComponentModel.DataAnnotations;

namespace API_Cursos.Entities;

public class Estudiante
{   
    [Key]
    public int IdEstudiante { get; set; }
    [Required]
    public required string Nombre { get; set; }

    [Required]
    public required string Apellido { get; set; }

    [Required]
    [Range(0,100,ErrorMessage ="El {0} debe ser de {1}-{2}")]
    public required float Promedio { get; set; }
    [Required]
    [Range(0,40,ErrorMessage ="El {0} debe ser de {1}-{2}")]
    public required float Edad { get; set; }
    [Required]
    public required string Grado { get; set; }
    [Required]
    public required bool Estado { get; set; }

    public List<Asignacion> Asignacion { get; set; } = new List<Asignacion>();

}

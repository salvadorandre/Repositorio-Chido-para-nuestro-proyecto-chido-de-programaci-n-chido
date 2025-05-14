using System;
using System.ComponentModel.DataAnnotations;

namespace API_Cursos.Entities;

public class Profesores
{
    [Key]
    public int IdProfesor { get; set; }
    [Required(ErrorMessage ="el campo {0} es requerido")]
    public required string Nombre { get; set; }
    [Required(ErrorMessage ="el campo {0} es requerido")]
    [Range(15,80,ErrorMessage ="El campo {0} tiene valores fuera del rango {1}-{2}")]
    public required int Edad { get; set; }
    [Required]
    [Range(1,100,ErrorMessage ="El campo {0} tiene que tener almenos {1} estudiantes y menos de {2} estudiantes")]
    public required int CapacidadEstudiantes { get; set; }
    [Required(ErrorMessage = "campo requerido")]
    public required bool Estado { get; set; }

    //TODO: Pendiente implementar metodos de navegacion
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Cursos.Entities;

public class Asignacion
{
    [Key]
    public int IDAsignacion { get; set; }   
    [Required]
    public required int EstudianteId { get; set; }
    [Required]
    public required int ProfesorCursoId { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime Fecha { get; set; }
    [Required]
    public required bool Estado { get; set; }
    [ForeignKey("EstudianteId")]
    public Estudiante? Estudiante { get; set; }
    [ForeignKey("ProfesorCursoId")]
    public ProfesorCurso? ProfesorCurso { get; set; }
}

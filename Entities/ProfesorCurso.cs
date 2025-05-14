using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API_Cursos.DTOs;

namespace API_Cursos.Entities;

public class ProfesorCurso
{
    [Key]
    public int IdProfesorCurso { get; set; }    
    [Required]
    public required int ProfesorId { get; set; } 
    [Required]
    public required int CursoId { get; set; }    
    [Required]
    public required bool Estado { get; set; }

    [ForeignKey("ProfesorId")]
    public Profesor? Profesor { get; set; }
    [ForeignKey("CursoId")]
    public Curso? Curso { get; set; }
    public List<Asignacion> Asignacion { get; set; } = new List<Asignacion>();


}

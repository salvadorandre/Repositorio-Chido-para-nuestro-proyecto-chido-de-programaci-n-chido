using System;
using System.ComponentModel.DataAnnotations;
using API_Cursos.Entities;

namespace API_Cursos.DTOs;

public class Curso
{
    [Key]
    public int IdCurso { get; set; }
    [Required]
    public required string Nombre { get; set; }
    [Required]
    public bool Estado { get; set; }
    public List<ProfesorCurso> ProfesorCurso { get; set; } = new List<ProfesorCurso>();

}

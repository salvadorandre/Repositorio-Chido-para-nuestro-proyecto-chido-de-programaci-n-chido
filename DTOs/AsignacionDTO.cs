using System;
using API_Cursos.Entities;

namespace API_Cursos.DTOs;

public class AsignacionDTO
{
    public int IdAsignacion { get; set; }
    public string? Curso { get; set; }
    public string? Profesor { get; set; }
    //public string? Estudiante { get; set; }
    public bool Estado { get; set; }
}

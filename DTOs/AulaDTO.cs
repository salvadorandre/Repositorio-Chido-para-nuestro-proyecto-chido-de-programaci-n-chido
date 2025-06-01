using System;
using API_Cursos.Entities;

namespace API_Cursos.DTOs;

public class AulaDTO
{
    public int IdAula { get; set; }
    public CursoDTO? Curso { get; set; }
    public ProfesorSampleDTO? Profesor { get; set; }
    public int? TotalEstudiantes { get; set; }

    public bool Estado { get; set; }
}

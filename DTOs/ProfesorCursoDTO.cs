using System;

namespace API_Cursos.DTOs;

public class ProfesorCursoDTO
{
    public int? idProfesorCurso { get; set; }
    public CursoDTO? curso { get; set; }
    public ProfesorDTO? profesor { get; set; }
    public bool Estado { get; set; }
}

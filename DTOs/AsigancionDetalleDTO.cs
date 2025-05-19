using System;

namespace API_Cursos.DTOs;

public class AsigancionDetalleDTO

{
    public int IdAsignacion { get; set; }
    public DateTime Fecha { get; set; }
    public CursoDTO? Curso { get; set; } 
    public ProfesorDTO? Profesor { get; set; }
    public EstudianteDTO? Estudiante { get; set; }
    public bool Estado { get; set; }
}



using System;

namespace API_Cursos.DTOs;

public class EstudianteDTO
{

    public int IdEstudiante { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public float Promedio { get; set; }
    public string? Grado { get; set; }
    public bool? Estado { get; set; }
    public IEnumerable<AsignacionDTO>? Asignacion { get; set; }
}

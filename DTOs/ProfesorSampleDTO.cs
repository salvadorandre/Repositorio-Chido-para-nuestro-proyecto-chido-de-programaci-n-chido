using System;
using API_Cursos.Entities;

namespace API_Cursos.DTOs;

public class ProfesorSampleDTO
{
     
    public int IdProfesor { get; set; }

    public  string? Nombre { get; set; }
    public  int CapacidadEstudiantes { get; set; }
   
}

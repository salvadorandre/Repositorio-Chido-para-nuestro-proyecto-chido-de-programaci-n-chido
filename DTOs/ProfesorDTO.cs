using System;
using API_Cursos.Entities;

namespace API_Cursos.DTOs;

public class ProfesorDTO
{
     
    public int IdProfesor { get; set; }

    public  string? Nombre { get; set; }


    public  int Edad { get; set; }

   
    public  int CapacidadEstudiantes { get; set; }
   
    public  bool Estado { get; set; }

    public IEnumerable<ProfesorCursoDTO>? ProfesorCurso { get; set; }
}

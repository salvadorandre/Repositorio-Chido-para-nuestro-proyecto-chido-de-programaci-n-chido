using System;
using API_Cursos.DTOs;
using API_Cursos.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_Cursos.Data;

public class AppDBContext:DbContext
{
    public AppDBContext(DbContextOptions options):base(options)
    {
        
    }

    public DbSet<Profesor> Profesor { get; set; }
    public DbSet<Estudiante> Estudiante { get; set; }
    public DbSet<Curso> Curso { get; set; }
    public DbSet<Asignacion> Asignacion { get; set; }
    public DbSet<ProfesorCurso> ProfesorCurso { get; set; }
}

using System;
using API_Cursos.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_Cursos.Data;

public class AppDBContext:DbContext
{
    public AppDBContext(DbContextOptions options):base(options)
    {
        
    }

    public DbSet<Profesores> Profesores { get; set; }
}

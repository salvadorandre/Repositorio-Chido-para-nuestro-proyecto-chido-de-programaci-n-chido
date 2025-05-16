using API_Cursos.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
#region Services
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticasCors", policy =>
    {
        policy
            .AllowAnyOrigin() 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>( options=>
    options.UseAzureSql("name=DefaultConnection")
);
builder.Services.AddControllers();


#endregion Services
var app = builder.Build();
#region Middlewares
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("PoliticasCors");
app.MapControllers();

#endregion Middlewares
app.Run();


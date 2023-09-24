using Microsoft.EntityFrameworkCore;
using MiProyectoApi.Datos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Habilitar CORS
app.UseCors(builder =>
{
    builder.AllowAnyOrigin() // Permitir solicitudes desde cualquier origen
           .AllowAnyMethod() // Permitir cualquier método HTTP
           .AllowAnyHeader(); // Permitir cualquier encabezado HTTP
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

using desafio_backend_uol.Data;
using desafio_backend_uol.Extensions;
using desafio_backend_uol.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddSingleton<CodinomeService>();
builder.Services.AddScoped<JogadorService>();

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite("Data source=jogadores.db");
});

var app = builder.Build();

app.UseJogadorApiEndpoints();

app.UseStaticFiles();

app.UseCors(op => op.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.Run();

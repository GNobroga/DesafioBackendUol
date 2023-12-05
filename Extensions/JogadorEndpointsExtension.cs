using desafio_backend_uol.Models;
using desafio_backend_uol.Services;
using Microsoft.AspNetCore.Mvc;

namespace desafio_backend_uol.Extensions;

public static class JogadorEndpointsExtension
{
    public static WebApplication UseJogadorApiEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/jogadores");

        api.MapGet("/", async (HttpContext context, [FromServices] JogadorService service) => {
            
            await context.Response.WriteAsJsonAsync(service.FindAllJogadores());
        });

        api.MapPost("/", async (HttpContext context, [FromBody] Jogador jogador, [FromServices] JogadorService service) => {
           try 
           {
                await context.Response.WriteAsJsonAsync(service.Create(jogador));
           }
           catch (Exception ex)
           {    
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { Titulo = "Error", Mensagem = ex.Message });
           }
        });

        api.MapPut("/{id:int}", async (HttpContext context, [FromRoute] int id, [FromBody] Jogador jogador, [FromServices] JogadorService service) => {
           try 
           {
                await context.Response.WriteAsJsonAsync(service.EditJogador(id, jogador));
           }
           catch (Exception ex)
           {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { Titulo = "Error", Mensagem = ex.Message });
           }
        });

        api.MapDelete("/{id:int}", async (HttpContext context, [FromRoute] int id, [FromServices] JogadorService service) => {
                await context.Response.WriteAsJsonAsync(new { removido = service.DeleteJogador(id) });
        });

        return app;
    }
}
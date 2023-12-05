using System.ComponentModel.DataAnnotations;

namespace desafio_backend_uol.Models;

public class Jogador 
{
    public int Id { get; set; }
    
    public string Nome { get; set; }

    public string Email { get; set; }

    public string? Telefone { get; set; }

    public string? Codinome { get; set; }

    [RegularExpression(@"(A Liga da Justiça|Os Vingadores)$")]
    public string Grupo { get; set; }

}
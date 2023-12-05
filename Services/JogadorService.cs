using desafio_backend_uol.Data;
using desafio_backend_uol.Models;

namespace desafio_backend_uol.Services;

public class JogadorService
{
    private readonly AppDbContext _context;

    private readonly CodinomeService _codinomeService;

    public JogadorService(AppDbContext context, CodinomeService codinomeService)
    {
        _context = context;
        _codinomeService = codinomeService;
    }

    public List<Jogador> FindAllJogadores()
    {
        return _context.Jogadores.ToList();
    }

    public Jogador Create(Jogador jogador)
    {   
        jogador.Id = 0;
        jogador.Codinome = GetCodinomeDisponivel(jogador.Grupo);

        if (string.IsNullOrEmpty(jogador.Nome)) 
            throw new Exception("O jogador precisa de um Nome");
        
        if (string.IsNullOrEmpty(jogador.Email)) 
            throw new Exception("O jogador precisa de um Email");
        
        if (jogador.Grupo == null || (!jogador.Grupo.Equals("Os Vingadores") && !jogador.Grupo.Equals("A Liga da Justiça"))) 
            throw new Exception("O grupo precisa ser Vingadores ou Justica");
        

        if (ExistEmail(jogador.Email))
            throw new Exception($"O email {jogador.Email} ja esta sendo usado");

        _context.Jogadores.Add(jogador);
        _context.SaveChanges();
        return jogador;
    }

    public Jogador EditJogador(int id, Jogador dados)
    {
        var jogador = _context.Jogadores.Find(id) ?? throw new Exception($"O Jogador com Id {id} nao existe");

        if (string.IsNullOrEmpty(jogador.Nome)) 
            throw new Exception("O jogador precisa de um Nome");
        
        if (string.IsNullOrEmpty(jogador.Email)) 
            throw new Exception("O jogador precisa de um Email");
        
        if (jogador.Grupo == null || (!jogador.Grupo.Equals("Os Vingadores") && !jogador.Grupo.Equals("A Liga da Justiça"))) 
            throw new Exception("O grupo precisa ser Vingadores ou Justica");


        if (ExistEmail(dados.Email) && dados.Email != jogador.Email)
        {
            throw new Exception($"O Email {dados.Email} ja esta sendo usado");
        }

        jogador.Email = dados.Email;
        jogador.Nome = dados.Nome;
        jogador.Telefone = dados.Telefone;
        jogador.Grupo = dados.Grupo;
        jogador.Codinome = GetCodinomeDisponivel(dados.Grupo, jogador.Codinome!);

        _context.SaveChanges();

        return jogador;
    }

    public bool DeleteJogador(int id)
    {
        var jogador = _context.Jogadores.Find(id);

        if (jogador != null)
        {
            _context.Jogadores.Remove(jogador);
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    private string GetCodinomeDisponivel(string grupo, string antigo = "")
    {
        List<Codinome> codinomes = grupo.Equals("A Liga da Justiça") ?
            _codinomeService.FindAllLigaDaJustica() : _codinomeService.FindAllVingadores();

        string codinomeDisponivel = string.Empty;

        foreach(var codinome in codinomes)
        {
            if (!_context.Jogadores.Any(j => j.Codinome!.Equals(codinome.Nome) && j.Grupo.Equals(grupo)))
            {
                codinomeDisponivel = codinome.Nome;
                break;
            }
        }

        if (string.IsNullOrEmpty(codinomeDisponivel) && !string.IsNullOrEmpty(antigo)) 
            return antigo;

        if (string.IsNullOrEmpty(codinomeDisponivel))
            throw new Exception($"Não há codinome disponível no Grupo {grupo}");
        

        return codinomeDisponivel;
    }

    private bool ExistEmail(string email)
    {
        return _context.Jogadores.Any(j => j.Email.Trim() == email.Trim());
    }
}

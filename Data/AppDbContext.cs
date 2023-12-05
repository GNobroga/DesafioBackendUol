using desafio_backend_uol.Models;
using Microsoft.EntityFrameworkCore;

namespace desafio_backend_uol.Data;

public class AppDbContext : DbContext
{
    public DbSet<Jogador> Jogadores { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}
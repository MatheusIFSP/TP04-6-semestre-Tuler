using Microsoft.EntityFrameworkCore;
using TP04.Api.Models;

namespace TP04.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opcoes) : DbContext(opcoes)
{
    public DbSet<Produto> Produtos => Set<Produto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>(entidade =>
        {
            entidade.HasKey(p => p.Id);
            entidade.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            entidade.Property(p => p.Descricao).HasMaxLength(500);
            entidade.Property(p => p.Preco).HasPrecision(18, 2);
        });
    }
}

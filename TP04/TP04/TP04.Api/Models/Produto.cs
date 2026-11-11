namespace TP04.Api.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
}

public record ProdutoDto(string? Nome, string? Descricao, decimal Preco, int Quantidade);

public record ValorEstoqueDto(decimal ValorTotal, string ValorFormatado);

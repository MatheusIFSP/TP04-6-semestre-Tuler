namespace TP04.Biblioteca;

public static class ValidadorProduto
{
    public static IReadOnlyList<string> Validar(string? nome, decimal preco, int quantidade)
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(nome))
            erros.Add("O nome é obrigatório.");
        else if (nome.Trim().Length > 100)
            erros.Add("O nome deve ter no máximo 100 caracteres.");

        if (preco <= 0)
            erros.Add("O preço deve ser maior que zero.");

        if (quantidade < 0)
            erros.Add("A quantidade em estoque não pode ser negativa.");

        return erros;
    }
}

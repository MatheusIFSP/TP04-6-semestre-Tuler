using System.Globalization;

namespace TP04.Biblioteca;

public static class CalculadoraEstoque
{
    private static readonly CultureInfo Cultura = new("pt-BR");

    public static decimal ValorTotal(decimal preco, int quantidade) => preco * quantidade;

    public static string FormatarMoeda(decimal valor) => valor.ToString("C", Cultura);
}

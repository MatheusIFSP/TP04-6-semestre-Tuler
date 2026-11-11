using TP04.Biblioteca;

namespace TP04.Tests;

public class BibliotecaTests
{
    [Fact]
    public void Validar_DadosValidos_NaoRetornaErros()
    {
        Assert.Empty(ValidadorProduto.Validar("Mouse", 50m, 3));
    }

    [Fact]
    public void Validar_TodosCamposInvalidos_RetornaTresErros()
    {
        Assert.Equal(3, ValidadorProduto.Validar(null, 0m, -1).Count);
    }

    [Fact]
    public void Validar_NomeMuitoLongo_RetornaErro()
    {
        Assert.Single(ValidadorProduto.Validar(new string((char)97, 101), 10m, 1));
    }

    [Theory]
    [InlineData(10.50, 4, 42.00)]
    [InlineData(99.99, 0, 0)]
    [InlineData(1, 1, 1)]
    public void ValorTotal_MultiplicaPrecoPelaQuantidade(double preco, int quantidade, double esperado)
    {
        Assert.Equal((decimal)esperado, CalculadoraEstoque.ValorTotal((decimal)preco, quantidade));
    }

    [Fact]
    public void FormatarMoeda_UsaCulturaBrasileira()
    {
        var texto = CalculadoraEstoque.FormatarMoeda(1234.5m).Replace((char)160, (char)32);
        Assert.Equal("R$ 1.234,50", texto);
    }
}
